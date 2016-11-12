using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        void Create(CommentDto comment, string gameKey);

        IEnumerable<CommentDto> GetByGame(string gameKey);
    }
}