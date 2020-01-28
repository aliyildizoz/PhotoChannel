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
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("add")]
        public IActionResult Add(Category category)
        {
            IDataResult<Category> result = _categoryService.Add(category);
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.IsSuccessful);
        }


        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            IDataResult<List<Category>> result = _categoryService.GetList();
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.IsSuccessful);
        }
    }
}