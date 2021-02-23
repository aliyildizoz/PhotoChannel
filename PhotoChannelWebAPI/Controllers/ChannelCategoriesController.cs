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
using PhotoChannelWebAPI.Extensions;

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

        [HttpGet]
        [Route("{channelId}/channel-categories")]
        public IActionResult GetChannelCategories(int channelId)
        {
            //Todo: channelId var mı kontrolü 

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

        [HttpPost]
        public IActionResult Post(ChannelCategoryForAddDto channelCategoryDto)
        {
            //Todo: channelId,categoryId var mı kontrolü 

            var channelCategory = _mapper.Map<ChannelCategory>(channelCategoryDto);
            IDataResult<ChannelCategory> dataResult = _channelCategoryService.Add(channelCategory);

            if (dataResult.IsSuccessful)
            {
                this.RemoveCacheByContains("api/channelcategories/" + channelCategoryDto.ChannelId);
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpPut]
        [Route("{channelId}")]
        public IActionResult Put(int channelId, ChannelCategoryForAddRangeDto channelCategoriesDto)
        {
            //Todo: channelId var mı kontrolü 

            if (channelId > 0)
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
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(ChannelCategoryForDeleteDto channelCategoryDto)
        {
            //Todo: channelId,categoryId var mı kontrolü 

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