using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        readonly ICategoryService _categoryService;
        readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;   
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryResource>> GetAllAsync()
        {
            var categories = await _categoryService.ListAsync();
            return  _mapper.Map<IEnumerable<CategoryResource>>(categories);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(SaveCategoryResource resource)
        {
            var category = _mapper.Map<Category>(resource);
            var response = await _categoryService.SaveAsync(category);

            if(!response.Success)
                return BadRequest(response);

            var categoryResource = _mapper.Map<CategoryResource>(response.Category);

            return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, SaveCategoryResource resource)
        {
            var category = _mapper.Map<Category>(resource);
            var response = await _categoryService.UpdateAsync(id, category);

            if(!response.Success)
                return BadRequest(response);

            var categoryResource = _mapper.Map<CategoryResource>(response.Category);

            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Category);
            return Ok(categoryResource);
        }
    }
}
