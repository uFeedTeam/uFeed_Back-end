using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreDto> GetAll();
    }
}
