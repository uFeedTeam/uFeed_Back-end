using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using uFeed.DAL.SocialService.Exceptions;
using uFeed.DAL.SocialService.Interfaces;
using uFeed.DAL.SocialService.Models.FacebookModel;
using uFeed.DAL.SocialService.Models.FacebookModel.Feed;
using uFeed.DAL.SocialService.Models.FacebookModel.Likes;
using uFeed.DAL.SocialService.Models.FacebookModel.User;
using uFeed.Entities;
using uFeed.Entities.Enums;
using uFeed.Entities.Social;
using uFeed.Entities.Social.Attach;
using Attachment = uFeed.Entities.Social.Attach.Attachment;
using Likes = uFeed.Entities.Social.Attach.Likes;
using Post = uFeed.Entities.Social.Post;

namespace uFeed.DAL.SocialService.Services.Facebook
{
    public class Api : ISocialApi
    {       
        private const string FacebookGetAccesstoken = "https://graph.facebook.com/v2.7/oauth/access_token";
        private const string AppId = "141340716272867";
        private const string RedirectUrl = "http://ufeed.azurewebsites.net/api/social/fbauth";
        private const string AppSecret = "18715401fbc856779f25f04595d7b20f";

        private readonly string _token;
        private readonly JavaScriptSerializer _serializer;

        public Api(string token, string code)
        {
            _serializer = new JavaScriptSerializer();
            _token = token ?? Login(code);
        }

        public List<Author> GetAllAuthors()
        {
            SerializedLikes likes = null;
            var result = new List<Author>();

            try
            {
                do
                {
                    var likesResult = likes == null
                        ? Request.ExecuteFacebookRequest("me/?fields=likes.limit(100){name,id,picture,link}", _token, "get")
                        : Request.SendGetRequest(likes.Likes.Paging.Next, null);

                    likes = _serializer.Deserialize<SerializedLikes>(likesResult);

                    if (likes.Likes == null)
                    {
                        continue;
                    }

                    result.AddRange(likes.Likes.Data.Select(item => new Author
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Photo = new Photo {Url = item.Picture.Data.Url},
                            Url = item.Link,
                            Source = Socials.Facebook
                        }));
                }
                while (likes.Likes != null);
            }
            catch (WebException e)
            {
                throw new SocialException("Cannot get all authors.\n" + e.Message);
            }    
                  
            return result;

        }

        public List<Author> GetAuthors(int page, int count)
        {
            var result = new List<Author>();

            try
            {
                if (count < 1 || page < 1)
                {
                    throw new WebException("Arguments 'count' and 'page' must be grater than 0");
                }

                string likesResult =
                    Request.ExecuteFacebookRequest("me/?fields=likes.limit(" + count + ").offset(" + count * page + "){name,id,picture,link}",
                        _token, "get");

                var likes = _serializer.Deserialize<SerializedLikes>(likesResult);

                result.AddRange(likes.Likes.Data.Select(item => new Author()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Photo = new Photo {Url = item.Picture.Data.Url},
                    Url = item.Link,
                    Source = Socials.Facebook
                }));
            }
            catch (WebException e)
            {
                throw new SocialException("Cannot get authors.\n" + e.Message);
            }

            return result;
        }

        public List<Post> GetFeed(Category category, int page, int countPosts)
        {
            var serializedResult = new List<SerializedFeed>();
            var result = new List<Post>();

            try
            {
                if (countPosts < 1 || page < 1)
                {
                    throw new WebException("Arguments 'countPosts' and 'page' must be grater than 0");
                }

                if (category == null)
                {
                    throw new WebException("Category cannot be null");
                }

                if (category.Authors == null || category.Authors.Count == 0)
                {
                    return result;
                }

                string ids = string.Empty;
                int count;

                for (count = 1; count <= category.Authors.Count; count++)
                {
                    ids += category.Authors.ElementAt(count - 1).AuthorId + ",";

                    if (count % 10 == 0)
                    {
                        serializedResult.AddRange(ExecuteFeedRequest(ids.Substring(0, ids.Length - 1), countPosts, page + countPosts));
                        ids = "";
                    }
                }

                if (count % 10 != 0 && count % 10 - 1 != 0)
                {
                    serializedResult.AddRange(ExecuteFeedRequest(ids.Substring(0, ids.Length - 1), countPosts, page + countPosts));
                }

                result = ConvertFeed(serializedResult);
            }
            catch (WebException e)
            {
                throw new SocialException("Cannot get feed.\n" + e.Message);
            }

            return result;
        }

        public string GetToken()
        {
            return _token;
        }

        public string GetUserId()
        {
            return string.Empty;
        }

        public UserInfo GetUserInfo()
        {
            var result = new UserInfo();

            try
            {                
                var userResult = Request
                    .ExecuteFacebookRequest("me/?fields=name,first_name,last_name,id,picture.height(500)", _token, "get");

                var user = _serializer.Deserialize<User>(userResult);

                result.Id = user.Id;
                result.Name = user.Name;
                result.Photo = new Photo {Url = user.Picture.Data.Url};
                result.Source = Socials.Facebook;
            }
            catch (WebException e)
            {
                throw new SocialException("Cannot get user info.\n" + e.Message);
            }
            return result;
        }

        private List<SerializedFeed> ExecuteFeedRequest(string ids, int countPosts, int offset)
        {
            string request = "?ids=" + ids
                             + "&fields=id,name,link,picture.type(normal),feed.limit("
                             + countPosts +
                             ").offset(" + offset + "){message,attachments,link,name,created_time,likes.limit(0).summary(true)}";

            string feedResult =
                Request.ExecuteFacebookRequest(request, _token, "get");

            //Converting this shit to JSON array of SerializedFeed
            feedResult = Regex.Replace(feedResult, "^{\"\\d+\":{", @"[{");
            feedResult = Regex.Replace(feedResult, "\"\\d+\":{", @"{");
            feedResult = feedResult.Substring(0, feedResult.Length - 1);
            feedResult += "]";

            return _serializer.Deserialize<List<SerializedFeed>>(feedResult);
        }

        private List<Post> ConvertFeed(List<SerializedFeed> serializedResult)
        {
            var result = new List<Post>();
            foreach (var feed in serializedResult)
            {
                if (feed.Feed == null)
                {
                    continue;
                }

                foreach (var post in feed.Feed.Data)
                {
                    var attachments = new List<Attachment>();
                    if (post.Attachments != null)
                    {
                        foreach (var fbAttach in post.Attachments.Data)
                        {
                            var attachment = new Attachment {Title = fbAttach.Title};

                            if (fbAttach.Type == "multi_share")
                            {
                                foreach (var share in fbAttach.Subattachments.Data)
                                {
                                    attachment = new Attachment
                                    {
                                        Title = share.Title,
                                        Link = new Link
                                        {
                                            Url = share.Url,
                                            Description = share.Description,
                                            Photo =
                                                share.Media != null ? new Photo {Url = share.Media.Image.Src} : null
                                        }
                                    };
                                    attachments.Add(attachment);
                                }
                                continue;
                            }

                            switch (fbAttach.Type)
                            {
                                case "video_autoplay":
                                case "video_share_youtube":
                                    attachment.Type = "video";
                                    attachment.Video = new Video
                                    {
                                        Url = fbAttach.Url,
                                        Photo =
                                            fbAttach.Media != null
                                                ? new Photo {Url = fbAttach.Media.Image.Src}
                                                : null,
                                        Description = fbAttach.Description
                                    };
                                    break;
                                case "photo":
                                case "cover_photo":
                                    attachment.Type = "photo";
                                    attachment.Photo = fbAttach.Media != null
                                        ? new Photo {Url = fbAttach.Media.Image.Src}
                                        : null;
                                    break;
                                case "share":
                                case "event":
                                    attachment.Type = "link";
                                    attachment.Link = new Link
                                    {
                                        Url = fbAttach.Url,
                                        Photo =
                                            fbAttach.Media != null
                                                ? new Photo {Url = fbAttach.Media.Image.Src}
                                                : null,
                                        Description = fbAttach.Description
                                    };
                                    break;
                                case "album":
                                case "new_album":
                                    attachment.Type = "album";
                                    var photos = new List<Photo>();
                                    foreach (var photo in fbAttach.Subattachments.Data)
                                    {
                                        photos.Add(new Photo {Url = photo.Media.Image.Src});
                                    }
                                    attachment.Album = new Album {Description = fbAttach.Description};
                                    attachment.Album.AddRange(photos);
                                    break;
                                case "animated_image_autoplay":
                                    attachment.Type = "doc";
                                    attachment.Document = new Document
                                    {
                                        Url = fbAttach.Url,
                                        Photo =
                                            fbAttach.Media != null
                                                ? new Photo {Url = fbAttach.Media.Image.Src}
                                                : null,
                                        Extension = "gif",
                                        Size = 0
                                    };
                                    break;
                            }
                            attachments.Add(attachment);
                        }
                    }
                    result.Add(new Post
                    {
                        Author =
                            new Author
                            {
                                Id = feed.Id,
                                Name = feed.Name,
                                Photo = new Photo {Url = feed.Picture.Data.Url},
                                Url = feed.Link
                            },
                        CreatedTime = DateTime.Parse(post.Created_time),
                        Url = post.Link,
                        Id = post.Id,
                        Message = post.Message,
                        Likes = new Likes
                        {
                            Count = post.Likes.Summary.Total_count,
                            IsLiked = post.Likes.Summary.Has_liked,
                            Url = "https://graph.facebook.com/v2.7/" + post.Id + 
                                "/likes?access_token=" + _token
                        },
                        Attachments = attachments
                    });
                }
            }
            return result;
        }

        private string Login(string code)
        {
            AccessToken accessToken;

            try {
                string accessTokenJson = Request.SendGetRequest(FacebookGetAccesstoken,
                    $"client_id={AppId}&redirect_uri={RedirectUrl}&client_secret={AppSecret}&code={code}");
                accessToken = _serializer.Deserialize<AccessToken>(accessTokenJson);
            }
            catch(WebException e)
            {
                throw new SocialException("Cannot get access token.\n" + e.Message);
            }

            return accessToken.Access_token;
        }
    }
}