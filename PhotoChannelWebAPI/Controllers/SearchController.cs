using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/search")]
    [ApiController]
    [LogFilter]
    public class SearchController : ControllerBase
    {
        private IMapper _mapper;
        private ISearchService _searchService;
        private ICountService _countService;

        public SearchController(IMapper mapper, ISearchService searchService, ICountService countService)
        {
            _mapper = mapper;
            _searchService = searchService;
            _countService = countService;
        }

        /// <summary>
        /// Gets search results by the text
        /// </summary>
        /// <param name="text">Query</param>
        /// <response code="404">If nothing can be found by the text.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("searchbytext/{text}")]
        public IActionResult SearchByText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var dataResult = _searchService.SearchByText(text);
                if (dataResult.IsSuccessful)
                {
                    var searchTextDto = new SearchTextDto
                    {
                        Channels = _mapper.Map<List<ChannelForListDto>>(dataResult.Data.Item2),
                        Users = _mapper.Map<List<UserForListDto>>(dataResult.Data.Item1),
                        Text = text
                    };
                    return Ok(searchTextDto);
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets search results by the category id
        /// </summary>
        /// <param name="categoryId">Query</param>
        /// <response code="404">If nothing can be found by the category.</response>
        /// <response code="404">If the category is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(ICategoryService), typeof(Category))]
        [HttpGet("searchbycategory/{categoryId}")]
        public IActionResult SearchByCategory(int categoryId)
        {
            var dataResult = _searchService.SearchByCategory(categoryId);
            if (dataResult.IsSuccessful)
            {
                var channelDto = _mapper.Map<List<SearchByCategoryDto>>(dataResult.Data);
                return Ok(channelDto);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets search results by the category ids
        /// </summary>
        /// <param name="categoryIds">Category Ids</param>
        /// <response code="400">If the category length is less than zero.</response>
        /// <response code="404">If nothing can be found by the category.</response>
        /// <response code="404">If the category is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("searchbymulticategory")]
        public IActionResult SearchByMultiCategory([FromQuery(Name = "categoryIds[]")] int[] categoryIds)
        {
            if (categoryIds.Length > 0)
            {
                if (categoryIds.Length == 1)
                {
                    var dataResult = _searchService.SearchByCategory(categoryIds[0]);
                    if (dataResult.IsSuccessful)
                    {
                        var channelDto = _mapper.Map<List<SearchByCategoryDto>>(dataResult.Data);
                        channelDto.ForEach(dto =>
                        {
                            dto.SubscribersCount = _countService.GetSubscriberCount(dto.Id).Data;
                        });
                        return Ok(channelDto);
                    }
                    return NotFound();
                }
                else
                {
                    var dataResult = _searchService.SearchByMultiCategory(categoryIds);
                    if (dataResult.IsSuccessful)
                    {
                        var channelDto = _mapper.Map<List<SearchByCategoryDto>>(dataResult.Data);
                        channelDto.ForEach(dto =>
                        {
                            dto.SubscribersCount = _countService.GetSubscriberCount(dto.Id).Data;
                        });
                        return Ok(channelDto);
                    }
                    return NotFound();
                }
            }

            return BadRequest();
        }
    }
}
