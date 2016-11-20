using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using uFeed.DAL.SocialService.Exceptions;
using uFeed.DAL.SocialService.Interfaces;
using uFeed.DAL.SocialService.Models.VkModel;
using uFeed.DAL.SocialService.Models.VkModel.Attach;
using uFeed.Entities;
using uFeed.Entities.Enums;
using uFeed.Entities.Social;
using uFeed.Entities.Social.Attach;

namespace uFeed.DAL.SocialService.Services.Vk
{
    public class Api : ISocialApi
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public long AccessTokenExpiresIn { get; set; }

        public Api(string token, string userId, string code)
        {
            if (!string.IsNullOrEmpty(token))
            {
                AccessToken = token;
                UserId = userId;
            }
            else
            {
                UserId = GetTokenAndUserId(code);
            }   
        }

        private string GetTokenAndUserId(string code)
        {
            var url =
                $@"https://oauth.vk.com/access_token?client_id=5494787&client_secret=8GmVpN3SZCUlnTHzYD3o&redirect_uri=http://ufeed.azurewebsites.net/api/social/vkauth&code={code}";
            var req = WebRequest.Create(url);
            var resp = req.GetResponse();

            var stream = resp.GetResponseStream();
            if (stream == null)
            {
                throw new SocialException("Cannot get response stream");
            }

            var sr = new StreamReader(stream);
            var streamText = sr.ReadToEnd();
            sr.Close();

            AccessToken = streamText.Split(':')[1].Split(',')[0].Trim('\"');
            var userId = streamText.Split(':')[3].Trim('}');

            return userId;
        }

        public string GetToken()
        {
            return AccessToken;
        }

        public string GetUserId()
        {
            return UserId;
        }

        public UserInfo GetUserInfo()
        {
            var user = new UserInfo();

            string requestString = $"https://api.vk.com/method/users.get?user_ids={UserId}&access_token={AccessToken}&fields=photo_100";
            var token = MakeRequest(requestString);

            token = token["response"][0];

            user.Id = token["uid"].ToString();
            user.Name = token["first_name"] + " " + token["last_name"];
            user.Photo = new Photo { Url = token["photo_100"].ToString() };
            user.Source = Socials.Vk;

            return user;
        }

        public List<Author> GetAllAuthors()
        {
            string requestString = $"https://api.vk.com/method/groups.get?owner_id={UserId}&access_token={AccessToken}&extended=1";
            var token = MakeRequest(requestString);
            token = token["response"];

            List<VkGroup> vkGroupList = token.Children().Skip(1).Select(c => c.ToObject<VkGroup>()).ToList();

            return ConvertToGeneralAuthorList(vkGroupList);
        }

        public List<Author> GetAuthors(int page, int count)
        {
            if (page < 1 || count < 1)
            {
                throw new SocialException("Invalid page or elements count");
            }

            string requestString =
               $@"https://api.vk.com/method/groups.get?owner_id={UserId}&access_token={AccessToken}&extended=1&count={count}&offset={(page-1)*count}";
            var token = MakeRequest(requestString);
            token = token["response"];
            List<VkGroup> vkGroupList = token.Children().Skip(1).Select(c => c.ToObject<VkGroup>()).ToList();

            return ConvertToGeneralAuthorList(vkGroupList);
        }

        public List<Post> GetFeed(Category category, int page, int count)
        {
            if (page < 1 || count < 1)
            {
                throw new SocialException("Invalid page or elements count");
            }
            
            var vkFeed = new List<VkWallPost>();
            var authorsCount = category.Authors.Count;
            var authorsOffset = 0;

            while (authorsCount > 0)
            {
                
                if (authorsCount > 25)
                {
                    vkFeed.AddRange(GetFeedLess25(category.Authors.ToList(), count, (page - 1) * count, 25, authorsOffset));
                    authorsCount -= 25;
                    authorsOffset += 25;
                }
                else
                {
                    vkFeed.AddRange(GetFeedLess25(category.Authors.ToList(), count, (page - 1) * count, authorsCount, authorsOffset));
                    authorsOffset += authorsCount;
                    authorsCount = 0;
                }
            }

            return ConvertToGeneralPostList(vkFeed);           
        }

        private static List<Author> ConvertToGeneralAuthorList(IReadOnlyList<VkGroup> vkGroupList)
        {
            var authorList = new List<Author>();

            for (var i = 0; i < vkGroupList.Count; i++)
            {
                authorList.Add(new Author
                {
                    Id = vkGroupList[i].Id,
                    Name = vkGroupList[i].Name,
                    Photo = new Photo
                    {
                        Url = vkGroupList[i].Photo
                    },
                    Url = "http://vk.com/" + vkGroupList[i].ScreenName,
                    Source = Socials.Vk
                });
            }

            return authorList;
        }

        private List<Post> ConvertToGeneralPostList(IEnumerable<VkWallPost> vkFeed)
        {
            var generalFeed = new List<Post>();
            List<Author> currentAuthorList = GetAllAuthors();

            foreach (var vkPost in vkFeed)
            {
                var generalPost = new Post
                {
                    Id = vkPost.Id,
                    Message = vkPost.Text,
                    CreatedTime = ConvertFromLinuxTime(vkPost.Date),
                    Likes = new Likes
                    {
                        Count = (int) vkPost.Likes.Count,
                        IsLiked = Convert.ToBoolean(vkPost.Likes.UserLikes),
                        Url =
                            $"https://api.vk.com/method/likes.add?access_token={AccessToken}&type=post&owner_id={UserId}&item_id={vkPost.Id}"
                    }
                };

                

                try
                {
                    generalPost.Author = vkPost.FromId[0].Equals('-') ? 
                        currentAuthorList.First(x => x.Id.Equals(vkPost.FromId.Substring(1))) : 
                        currentAuthorList.First(x => x.Id.Equals(vkPost.FromId));
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                generalPost.Url =
                    $"https://vk.com/{generalPost.Author.Url.Substring(14)}?w=wall{vkPost.FromId}_{vkPost.Id}";

                if (vkPost.Attachments != null)
                {
                    generalPost.Attachments = MakeAttachmnetList(vkPost.Attachments);
                }

                generalFeed.Add(generalPost);
            }
            return generalFeed;
        }

        private static Attachment MakeDocumentAttachment(VkAttachment vkAttachment)
        {
            var attachment = new Attachment
            {
                Type = "Document",
                Title = vkAttachment.Doc.Title,
                Document = new Document
                {
                    Url = vkAttachment.Doc.Url,
                    Size = (int) vkAttachment.Doc.Size,
                    Extension = vkAttachment.Doc.Ext,
                }
            };

            if (vkAttachment.Doc.Preview != null)
            {
                attachment.Photo = new Photo
                {
                    Url = vkAttachment.Doc.Preview.Photo.Sizes.Last().Src
                };
            }

            return attachment;
        }

        private static Attachment MakeVideoAttachment(VkAttachment vkAttachment)
        {
            return new Attachment
            {
                Type = "Video",
                Title = vkAttachment.Video.Title,
                Video = new Video
                {
                    Description = vkAttachment.Video.Description,
                    Photo = new Photo
                    {
                        Url = vkAttachment.Video.Photo640
                    },
                    Url = $"https://vk.com/video{vkAttachment.Video.OwnerId}_{vkAttachment.Video.Id}"
                }
            };
        }

        private static Attachment MakePollAttachment(VkAttachment vkAttachment)
        {
            var pollAttachment = new Attachment
            {
                Type = "Poll",
                Title = vkAttachment.Poll.Question,
                Poll = new Poll
                {
                    Votes = vkAttachment.Poll.Votes,
                    AnswerId = vkAttachment.Poll.AnswerId
                }
            };
            pollAttachment.Poll.Answers = new List<PollAnswer>();
            foreach (var answer in vkAttachment.Poll.Answers)
            {
                pollAttachment.Poll.Answers.Add(new PollAnswer
                {
                    Id = answer.Id,
                    Votes = answer.Votes,
                    Rate = answer.Rate
                });
            }

            return pollAttachment;
        }

        private static Attachment MakeLinkAttachment(VkAttachment vkAttachment)
        {
            var linkAttach = new Attachment
            {
                Type = "Link",
                Title = vkAttachment.Link.Title
            };

            var link = new Link
            {
                Description = vkAttachment.Link.Description,
                Url = vkAttachment.Link.Url,
            };

            if (vkAttachment.Link.Photo != null)
            {
                link.Photo = new Photo
                {
                    Url = vkAttachment.Link.Photo.Photo807
                };
            }

            linkAttach.Link = link;

            return linkAttach;
        }

        private static Attachment MakeAlbumAttachment(IReadOnlyList<Attachment> photoAttachmentList)
        {
            var attach = new Attachment
            {
                Type = "Album",
                Title = photoAttachmentList[0].Title,
                Album = new Album()
            };


            foreach (var photoAttach in photoAttachmentList)
            {
                attach.Album.Add(photoAttach.Photo);
            }

            return attach;
        }

        private static Attachment MakePhotoAttachment(VkAttachment vkAttachment)
        {
            return new Attachment
            {
                Type = "Photo",
                Title = vkAttachment.Photo.Text,
                Photo = new Photo
                {
                    Url = vkAttachment.Photo.Photo604
                }
            };
        }

        private static List<Attachment> MakeAttachmnetList(IEnumerable<VkAttachment> vkAttachmentList)
        {
            var attachmentList = new List<Attachment>();
            var photoAttachmentList = new List<Attachment>();
            foreach (var attach in vkAttachmentList)
            {
                switch (attach.Type)
                {
                    case "video":
                        attachmentList.Add(MakeVideoAttachment(attach));
                        break;
                    case "doc":
                        attachmentList.Add(MakeDocumentAttachment(attach));
                        break;
                    case "link":
                        attachmentList.Add(MakeLinkAttachment(attach));
                        break;
                    case "poll":
                        attachmentList.Add(MakePollAttachment(attach));
                        break;
                    case "audio":
                        //throw new NotImplementedException();
                        break;
                    case "photo":
                        photoAttachmentList.Add(MakePhotoAttachment(attach));
                        break;
                }
            }

            if (photoAttachmentList.Count == 1)
            {
                attachmentList.Add(new Attachment
                {
                    Type = "Photo",
                    Title = photoAttachmentList[0].Title,
                    Photo = photoAttachmentList[0].Photo
                });
            }
            else if (photoAttachmentList.Count > 1)
            {
                attachmentList.Add(MakeAlbumAttachment(photoAttachmentList));
            }

            return attachmentList;
        }

        private IEnumerable<VkWallPost> GetFeedLess25(List<SocialAuthor> authors, int countPosts,int feedOffset, int countAuthors, int authorsOffset)
        {
            var offsetedAythorList = authors.GetRange(0 + authorsOffset, countAuthors);
            var executeMethodText = GetExecuteMethodTextForFeed(offsetedAythorList,countPosts, feedOffset);   
            string requestString =
                $"https://api.vk.com/method/execute?v=5.52&access_token={AccessToken}&code={executeMethodText}";
            var token = MakeRequest(requestString);
            IEnumerable<JToken> tokenArr = token["response"].Children()["items"].Children();

            var vkFeed = new List<VkWallPost>();
            FillFeedFromJTooken(vkFeed, tokenArr);

            return vkFeed;
        }

        private static void FillFeedFromJTooken(List<VkWallPost> vkGroupList, IEnumerable<JToken> tokenArr)
        {
            try
            {
                foreach (var jtoken in tokenArr)
                {
                    vkGroupList.Add(jtoken.ToObject<VkWallPost>());
                }
            }
            catch (InvalidOperationException)
            {
                var validElementCounter = 1;
                List<JToken> tempList;

                var counter = 26;
                while (counter-- != 0)
                {
                    try
                    {
                        tokenArr.Take(validElementCounter).ToList();
                        validElementCounter++;
                    }
                    catch (InvalidOperationException)
                    {
                        tempList = tokenArr.Take(validElementCounter - 1).Skip(1).ToList();

                        foreach (var jtoken in tempList)
                        {
                            vkGroupList.Add(jtoken.ToObject<VkWallPost>());
                        }
                        break;
                    }
                }
            }
        }

        private static string GetExecuteMethodTextForFeed(IEnumerable<SocialAuthor> authors, int countPosts, int feedOffset)
        {
            var rez = authors.Aggregate(@"var posts = {};", 
                (current, t) => current + ("posts.push(API.wall.get({ \"count\": " + countPosts + ", \"owner_id\":-" + t.AuthorId + ", \"offset\":" + feedOffset) + " }));");
            return rez += "return posts;";
        }

        private static JToken MakeRequest(string requestString)
        {
            var request = WebRequest.Create(requestString);
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            responseFromServer = HttpUtility.HtmlDecode(responseFromServer);
            return JToken.Parse(responseFromServer);
        }

        private static DateTime ConvertFromLinuxTime(long linuxDateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(linuxDateTime);
        }
    }
}