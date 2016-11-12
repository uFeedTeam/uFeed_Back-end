using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        void Create(GameDto game);

        void Update(GameDto game);

        void Delete(int id);

        IEnumerable<GameDto> GetAll();

        IEnumerable<GameDto> GetByGenre(string genreName);

        IEnumerable<GameDto> GetByPlatform(string platformType);

        GameDto Get(string key);
    }
}