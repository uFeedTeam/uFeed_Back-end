using System.Collections.Generic;
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
            var categories = _unitOfWork.Categories.Find(x => x.ClientProfileId == userId);
            var categoriesDto = Mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return categoriesDto;
        }
    }
}
