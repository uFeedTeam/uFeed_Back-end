using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class CategoryController: BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("category/new")]
        public IHttpActionResult Create(CategoryViewModel categoryViewModel)
        {
            categoryViewModel.UserId = CurrentUser.Id;
            var categoryDto = Mapper.Map<CategoryDTO>(categoryViewModel);
            _categoryService.Create(categoryDto);

            return Ok();
        }

        [HttpPost]
        [Route("category/update")]
        public IHttpActionResult Update(CategoryViewModel categoryViewModel)
        {
            try
            {
                var categoryDto = Mapper.Map<CategoryDTO>(categoryViewModel);
                _categoryService.Update(categoryDto);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("category/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var categoryDto = _categoryService.Get(id);
                var categoryViewModel = Mapper.Map<CategoryViewModel>(categoryDto);

                return Json(categoryViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("categories")]
        public IHttpActionResult GetByUserId()
        {
            IEnumerable<CategoryDTO> categoriesDto = _categoryService.GetByUserId(CurrentUser.Id);
            var categoriesViewModel = Mapper.Map<IEnumerable<CategoryViewModel>>(categoriesDto);

            return Json(categoriesViewModel);
        }

        [HttpGet]
        [Route("category/{id}/delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _categoryService.Delete(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}