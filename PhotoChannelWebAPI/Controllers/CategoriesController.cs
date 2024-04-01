using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;
using IResult = Core.Utilities.Results.IResult;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [LogFilter]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ContainsFilter(typeof(ICategoryService), typeof(Category),nameof(Category.Id))]
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Add(Category category)
        {
            IResult result = _categoryService.Add(category);
            if (result.IsSuccessful)
            {
                this.RemoveCache();
                return Ok(result.Message);
            }

            return BadRequest(result.IsSuccessful);
        }
        /// <summary>
        /// Gets list of categories.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetAll()
        {
            IDataResult<List<Category>> result = _categoryService.GetList();
            if (result.IsSuccessful)
            {
                this.CacheFill(result.Data, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(12),
                    SlidingExpiration = TimeSpan.FromHours(2)
                });
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

    }
}