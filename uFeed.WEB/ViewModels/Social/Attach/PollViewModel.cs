using System.Collections.Generic;

namespace uFeed.WEB.ViewModels.Social.Attach
{
    public class PollViewModel
    {
        public long Votes { get; set; }

        public string AnswerId { get; set; }

        public List<PollAnswerViewModel> Answers { get; set; }
    }
}