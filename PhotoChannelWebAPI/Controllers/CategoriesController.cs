using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Category category)
        {
            IResult result = _categoryService.Add(category);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.IsSuccessful);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IDataResult<List<Category>> result = _categoryService.GetList();
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("{categoryId}/channels")]
        public IActionResult GetChannels(int categoryId)
        {
            IDataResult<List<Channel>> result = _categoryService.GetChannels(categoryId);
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}