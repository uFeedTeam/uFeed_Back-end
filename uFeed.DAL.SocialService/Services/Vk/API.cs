using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using uFeed.DAL.SocialService.Models.VkModel;
using uFeed.DAL.SocialService.Models.VkModel.Attach;
using uFeed.Entities;
using uFeed.Entities.Social;
using uFeed.Entities.Social.Attach;

namespace uFeed.DAL.SocialService.Services.Vk
{
    public class Api
    {

        public UserInfo User
        {
            get{ return (UserInfo)_sessionState["vkUser"]; }
            set { _sessionState["vkUser"] = value; }
        }
        public string AccessToken
        {
            get { return (string)_sessionState["vkAccessToken"]; }
            set { _sessionState["vkAccessToken"] = value; }
        }
        public long AccessTokenExpiresIn
        {
            get { return (long)_sessionState["vkAccessTokenExpiresIn"]; }
            set { _sessionState["vkAccessTokenExpiresIn"] = value; }
        }

        private int CountAuthorsStep
        {
            get
            {
                if(_sessionState["vkCountAuthorStep"] != null)
                    return (int)_sessionState["vkCountAuthorStep"];
                return 0;
            }
            set { _sessionState["vkCountAuthorStep"] = value; }
        }
        private int CountAuthorsOffset
        {
            get
            {
                if(_sessionState["vkAccessTokenExpiresIn"] != null) 
                    return (int)_sessionState["vkAccessTokenExpiresIn"];
                return 0;
            }
            set { _sessionState["vkAccessTokenExpiresIn"] = value; }
        }

        private int CountFeedStep
        {
            get
            {
                if(_sessionState["vkCountFeedStep"] != null)
                    return (int)_sessionState["vkCountFeedStep"];
                return 0;
            }
            set { _sessionState["vkCountFeedStep"] = value; }
        }
        private int CountFeedOffset
        {
            get
            {
                if(_sessionState["vkCountFeedOffset"]!=null)
                    return (int)_sessionState["vkCountFeedOffset"];
                return 0;
            }
            set { _sessionState["vkCountFeedOffset"] = value; }
        }
        private List<Author> CurrentAuthorList
        {
            get { return (List<Author>)_sessionState["vkCurrentAuthorList"]; }
            set { _sessionState["vkCurrentAuthorList"] = value; }
        }

        private readonly HttpSessionStateBase _sessionState;


        public Api(string accessToken, string userId, int expiresIn, HttpSessionStateBase sessionState)
        {
            _sessionState = sessionState;

            AccessToken = accessToken;
            AccessTokenExpiresIn = expiresIn;
        
            FormUserInfo(userId);
        }

        public UserInfo GetUserInfo()
        {
            return User;
        }

        public List<Author> GetAllAuthors()
        {
            string requestString = $"https://api.vk.com/method/groups.get?owner_id={User.Id}&access_token={AccessToken}&extended=1";
            JToken token = MakeRequest(requestString);
            token = token["response"];

            var vkGroupList = token.Children().Skip(1).Select(c => c.ToObject<VkGroup>()).ToList();

            return ConvertToGeneralAuthorList(vkGroupList);

        }

        public List<Author> GetAuthors(int countAuthors)
        {
            CountAuthorsStep = countAuthors;

            string requestString = $"https://api.vk.com/method/groups.get?owner_id={User.Id}&access_token={AccessToken}&extended=1&count={countAuthors}";
            var token = MakeRequest(requestString);
            token = token["response"];

            var vkGroupList = token.Children().Skip(1).Select(c => c.ToObject<VkGroup>()).ToList();

            return ConvertToGeneralAuthorList(vkGroupList);

        }

        public void GetNextAuthors(List<Author> authorList)
        {
            if (CountAuthorsStep == 0)
            {
                throw new Exception("User GetAuthors() before GetNextAuthors()");
            }
            CountAuthorsOffset += CountAuthorsStep;

            string requestString =
                $"https://api.vk.com/method/groups.get?owner_id={User.Id}&access_token={AccessToken}&extended=1&count={CountAuthorsStep}&offset={CountAuthorsOffset}";
            var token = MakeRequest(requestString);
            token = token["response"];

            var vkGroupList = token.Children().Skip(1).Select(c => c.ToObject<VkGroup>()).ToList();

            authorList.AddRange(ConvertToGeneralAuthorList(vkGroupList));

        }

        public List<Post> GetFeed(Category category, int countPosts)
        {
            CountFeedStep = countPosts;
            //CurrentAuthorList = category.Authors.ToList();

            foreach (var author in category.Authors)
            {
                CurrentAuthorList.Add(new Author {Id = author.AuthorId});
            }
             
            var vkFeed = new List<VkWallPost>();
            var authorsCount = CurrentAuthorList.Count;
            var authorsOffset = 0;

            while (authorsCount > 0)
            {
                if (authorsCount > 25)
                {
                    vkFeed.AddRange(GetFeedLess25(CurrentAuthorList, countPosts, 0, 25, authorsOffset));
                    authorsCount -= 25;
                    authorsOffset += 25;
                }
                else
                {
                    vkFeed.AddRange(GetFeedLess25(CurrentAuthorList, countPosts, 0, authorsCount, authorsOffset));
                    authorsOffset += authorsCount;
                    authorsCount = 0;
                }
            }

            return ConvertToGeneralPostList(vkFeed);
        }

        public void GetNextFeed(List<Post> feed)
        {
            if (CountFeedStep == 0)
            {
                throw new Exception("User GetFeed() before GetNextFeed()");
            }
            CountFeedOffset += CountFeedStep;

            List<VkWallPost> vkFeed = new List<VkWallPost>();
            int authorsCount = CurrentAuthorList.Count;
            int authorsOffset = 0;

            while (authorsCount > 0)
            {
                if (authorsCount > 25)
                {
                    vkFeed.AddRange(GetFeedLess25(CurrentAuthorList, CountFeedStep, CountFeedOffset, 25, authorsOffset));
                    authorsCount -= 25;
                    authorsOffset += 25;
                }
                else
                {
                    vkFeed.AddRange(GetFeedLess25(CurrentAuthorList, CountFeedStep, CountFeedOffset, authorsCount, authorsOffset));
                    authorsOffset += authorsCount;
                    authorsCount = 0;
                }
            }

            var generalFeed = ConvertToGeneralPostList(vkFeed);

            foreach (var post in generalFeed)
            {
                if (feed.Exists(x => x.Id.Equals(post.Id)))
                    generalFeed.Remove(post);
            }

            feed.AddRange(generalFeed);
        }

        private static List<Author> ConvertToGeneralAuthorList(List<VkGroup> vkGroupList)
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
                    Url = "http://vk.com/" + vkGroupList[i].ScreenName
                });
            }

            return authorList;
        }

        private List<Post> ConvertToGeneralPostList(List<VkWallPost> vkFeed)
        {
            var generalFeed = new List<Post>();

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
                            $"https://api.vk.com/method/likes.add?access_token={AccessToken}&type=post&owner_id={User.Id}&item_id={vkPost.Id}"
                    }
                };

                try
                {
                    if (vkPost.FromId[0].Equals('-'))
                    {
                        generalPost.Author = CurrentAuthorList.First(x => x.Id.Equals(vkPost.FromId.Substring(1)));
                    }
                    else
                    {
                        generalPost.Author = CurrentAuthorList.First(x => x.Id.Equals(vkPost.FromId));
                    }
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

        private static Attachment MakeAlbumAttachment(List<Attachment> photoAttachmentList)
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

        private List<Attachment> MakeAttachmnetList(VkAttachment[] vkAttachmentList)
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
                attachmentList.Add(new Attachment()
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

        private IEnumerable<VkWallPost> GetFeedLess25(List<Author> authors, int countPosts,int feedOffset, int countAuthors, int authorsOffset)
        {
            string executeMethodText = GetExecuteMethodTextForFeed(authors.GetRange(0 + authorsOffset, countAuthors),
                countPosts, feedOffset);   
            string requestString =
                $"https://api.vk.com/method/execute?v=5.52&access_token={AccessToken}&code={executeMethodText}";
            var token = MakeRequest(requestString);
            IEnumerable<JToken> tokenArr = token["response"].Children()["items"].Children();

            var vkFeed = new List<VkWallPost>();
            FillFeedFromJTooken(vkFeed, tokenArr);

            //getFeedTest(vkFeed, countPosts);
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
                int validElementCounter = 1;
                List<JToken> tempList;

                int counter = 26;
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

                        foreach (JToken jtoken in tempList)
                        {
                            vkGroupList.Add(jtoken.ToObject<VkWallPost>());
                        }
                        break;
                    }
                }
            }
        }

        private static string GetExecuteMethodTextForFeed(List<Author> authors, int countPosts, int feedOffset)
        {
            var rez = "var posts = {};";
            for (int i = 0; i < authors.Count; i++)
            {
                rez += "posts.push(API.wall.get({ \"count\": " + countPosts + ", \"owner_id\":-" + authors[i].Id + ", \"offset\":" + feedOffset + " }));";
            }
            return rez += "return posts;";
        }

        private void FormUserInfo(string userId)
        {
            User = new UserInfo();

            string requestString = $"https://api.vk.com/method/users.get?user_ids={userId}&access_token={AccessToken}&fields=photo_100";
            var token = MakeRequest(requestString);
            token = token["response"][0];

            User.Id = token["uid"].ToString();
            User.Name = token["first_name"] + " " + token["last_name"];
            User.Photo = new Photo { Url = token["photo_100"].ToString() };
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

        #region Tests
        private void GetFeedTest(List<VkWallPost> feedList, int countPosts)
        {
            var feedDict = new Dictionary<string, int>();

            foreach (var wallPost in feedList)
            {
                int tempValue;
                feedDict.TryGetValue(wallPost.FromId, out tempValue);
                feedDict[wallPost.FromId] = ++tempValue;
            }

            var problemFeed = feedDict.Where(elem => elem.Value != countPosts).ToDictionary(elem => elem.Key, elem => elem.Value);
        }
        #endregion
    }
}