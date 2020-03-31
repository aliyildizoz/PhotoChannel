using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channelcategories")]
    [ApiController]
    public class ChannelCategoriesController : ControllerBase
    {
        private IChannelCategoryService _channelCategoryService;
        private IMapper _mapper;
        public ChannelCategoriesController(IChannelCategoryService channelCategoryService, IMapper mapper)
        {
            _channelCategoryService = channelCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{categoryId}/category-channels")]
        public IActionResult GetPhotoLikes(int categoryId)
        {
            IDataResult<List<Channel>> dataResult = _channelCategoryService.GetCategoryChannels(categoryId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpGet]
        [Route("{channelId}/channel-categories")]
        public IActionResult GetLikePhotos(int channelId)
        {
            IDataResult<List<Category>> dataResult = _channelCategoryService.GetChannelCategories(channelId);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpPost]
        public IActionResult Post(ChannelCategoryForAddDto channelCategoryDto)
        {
            var channelCategory = _mapper.Map<ChannelCategory>(channelCategoryDto);
            IDataResult<ChannelCategory> dataResult = _channelCategoryService.Add(channelCategory);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpPost]
        public IActionResult Post(ChannelCategoryForAddRangeDto channelCategoriesDto)
        {
            ChannelCategory[] channelCategories = new ChannelCategory[channelCategoriesDto.CategoryIds.Length];

            for (int i = 0; i < channelCategoriesDto.CategoryIds.Length; i++)
            {
                channelCategories[i] = new ChannelCategory { ChannelId = channelCategoriesDto.ChannelId, CategoryId = channelCategoriesDto.CategoryIds[i] };
            }

            IResult result = _channelCategoryService.AddRange(channelCategories);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpDelete]
        public IActionResult Delete(ChannelCategoryForDeleteDto channelCategoryDto)
        {
            var channelCategory = _mapper.Map<ChannelCategory>(channelCategoryDto);
            IResult result = _channelCategoryService.Delete(channelCategory);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}