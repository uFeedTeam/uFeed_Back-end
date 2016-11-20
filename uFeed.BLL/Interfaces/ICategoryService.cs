using System.Collections.Generic;
using uFeed.BLL.DTO;

namespace uFeed.BLL.Interfaces
{
    public interface ICategoryService
    {
        void Create(CategoryDTO categoryDto);

        void Update(CategoryDTO categoryDto);

        void Delete(int id);

        CategoryDTO Get(int id);

        IEnumerable<CategoryDTO> GetByUserId(int userId);
    }
}
