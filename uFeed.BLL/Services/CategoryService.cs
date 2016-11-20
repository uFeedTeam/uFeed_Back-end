using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.DAL.Interfaces;
using uFeed.Entities;

namespace uFeed.BLL.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(CategoryDTO categoryDto)
        {
            var category = Mapper.Map<Category>(categoryDto);

            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(categoryDto.UserId);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException($"User with id = {categoryDto.UserId} wasn't found", "ClientProfile");
            }

            category.ClientProfileId = clientProfile.Id;

            _unitOfWork.Categories.Create(category);
            _unitOfWork.Save();
        }

        public void Update(CategoryDTO categoryDto)
        {
            var updatingCategory = _unitOfWork.Categories.Get(categoryDto.Id);

            if (updatingCategory == null)
            {
                throw new EntityNotFoundException("Category doesn't exist", "Category");
            }

            foreach (var id in updatingCategory.Authors.Select(author => author.Id).ToList())
            {
                _unitOfWork.SocialAuthors.Delete(id);
            }

            updatingCategory.Authors.Clear();
            Mapper.Map(categoryDto, updatingCategory);

            _unitOfWork.Categories.Update(updatingCategory);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            if (_unitOfWork.Categories.Get(id) == null)
            {
                throw new EntityNotFoundException("Cannot find such category", "Category");
            }

            _unitOfWork.Categories.Delete(id);
            _unitOfWork.Save();
        }

        public CategoryDTO Get(int id)
        {
            var category = _unitOfWork.Categories.Get(id);
            if (category == null)
            {
                throw new EntityNotFoundException("Cannot find such category", "Category");
            }

            var categoryDto = Mapper.Map<CategoryDTO>(category);

            return categoryDto;
        }

        public IEnumerable<CategoryDTO> GetByUserId(int userId)
        {
            var categories = _unitOfWork.Categories.Find(x => x.User.UserId == userId);
            var categoriesDto = Mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return categoriesDto;
        }
    }
}
