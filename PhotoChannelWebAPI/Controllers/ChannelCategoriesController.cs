using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channelcategories")]
    [ApiController]
    [LogFilter]
    public class ChannelCategoriesController : ControllerBase
    {
        private IChannelCategoryService _channelCategoryService;
        private IMapper _mapper;
        public ChannelCategoriesController(IChannelCategoryService channelCategoryService, IMapper mapper)
        {
            _channelCategoryService = channelCategoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns channel list by the category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <response code="404">If the category is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(ICategoryService), typeof(Category))]
        [HttpGet]
        [Route("{categoryId}/category-channels")]
        public IActionResult GetCategoryChannels(int categoryId)
        {
            IDataResult<List<Channel>> dataResult = _channelCategoryService.GetCategoryChannels(categoryId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }
        /// <summary>
        /// Returns category list by the channel id
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <response code="404">If the channel is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Route("{channelId}/channel-categories")]
        public IActionResult GetChannelCategories(int channelId)
        {
            IDataResult<List<Category>> dataResult = _channelCategoryService.GetChannelCategories(channelId);

            if (dataResult.IsSuccessful)
            {
                if (dataResult.Data.Count > 0)
                {
                    this.CacheFill(dataResult.Data);
                }
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        /// <summary>
        /// Adds a category to the channel
        /// </summary>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the channel or the category is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(ICategoryService), typeof(Category), nameof(ChannelCategoryForAddDto.CategoryId))]
        [ContainsFilter(typeof(IChannelService), typeof(Channel), nameof(ChannelCategoryForAddDto.ChannelId))]
        [Authorize]
        [HttpPost]
        public IActionResult Post(ChannelCategoryForAddDto channelCategoryDto)
        {
            var channelCategory = _mapper.Map<ChannelCategory>(channelCategoryDto);
            IDataResult<ChannelCategory> dataResult = _channelCategoryService.Add(channelCategory);

            if (dataResult.IsSuccessful)
            {
                this.RemoveCacheByContains("api/channelcategories/" + channelCategoryDto.ChannelId);
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        /// <summary>
        /// Adds multiple categories to the channel
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="channelCategoriesDto"></param>
        /// <response code="400">If the categories not added</response>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the channel not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpPut]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Put(int channelId, ChannelCategoryForAddRangeDto channelCategoriesDto)
        {
            ChannelCategory[] channelCategories = new ChannelCategory[channelCategoriesDto.CategoryIds.Length];

            for (int i = 0; i < channelCategoriesDto.CategoryIds.Length; i++)
            {
                channelCategories[i] = new ChannelCategory { ChannelId = channelId, CategoryId = channelCategoriesDto.CategoryIds[i] };
            }

            IResult result = _channelCategoryService.AddRange(channelCategories);

            if (result.IsSuccessful)
            {
                this.RemoveCache();
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete a category from the channel
        /// </summary>
        /// <param name="channelCategoryDto"></param>
        /// <response code="400">If the channel or the category not added</response>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the channel or the category not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel), nameof(ChannelCategoryForDeleteDto.ChannelId))]
        [ContainsFilter(typeof(ICategoryService), typeof(Category), nameof(ChannelCategoryForDeleteDto.CategoryId))]
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(ChannelCategoryForDeleteDto channelCategoryDto)
        {
            var channelCategory = _mapper.Map<ChannelCategory>(channelCategoryDto);
            IResult result = _channelCategoryService.Delete(channelCategory);

            if (result.IsSuccessful)
            {
                this.RemoveCacheByContains("api/channelcategories/" + channelCategoryDto.ChannelId);
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}