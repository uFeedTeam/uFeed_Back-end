using System.Collections.Generic;

namespace uFeed.DAL.Entities.Social.Attach
{
    public class Poll
    {
        public long Votes { get; set; }

        public string AnswerId { get; set; }

        public List<PollAnswer> Answers { get; set; }
    }
}