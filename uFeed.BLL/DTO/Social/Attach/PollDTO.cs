using System.Collections.Generic;

namespace uFeed.BLL.DTO.Social.Attach
{
    public class PollDTO
    {
        public long Votes { get; set; }

        public string AnswerId { get; set; }

        public List<PollAnswerDTO> Answers { get; set; }
    }
}