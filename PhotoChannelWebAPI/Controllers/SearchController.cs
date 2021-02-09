using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/search")]
    [ApiController]
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
        [HttpGet("searchbycategory/{categoryId}")]
        public IActionResult SearchByCategory(int categoryId)
        {
            if (categoryId > 0)
            {
                var dataResult = _searchService.SearchByCategory(categoryId);
                if (dataResult.IsSuccessful)
                {
                    var channelDto = _mapper.Map<List<SearchByCategoryDto>>(dataResult.Data);
                    return Ok(channelDto);
                }

                return NotFound();
            }
            return BadRequest();
        }

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
