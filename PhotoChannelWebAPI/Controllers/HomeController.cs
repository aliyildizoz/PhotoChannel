using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/home")]
    [ApiController]
    [LogFilter]
    public class HomeController : ControllerBase
    {
        private IHomeService _homeService;
        private IMapper _mapper;
        private ICountService _countService;
        public HomeController(IHomeService homeService, IMapper mapper, ICountService countService)
        {
            _homeService = homeService;
            _mapper = mapper;
            _countService = countService;
        }
        /// <summary>
        /// Gets the best channels
        /// </summary>
        /// <returns></returns>
        /// <response code="200">The best channels</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("mostchannels")]
        public IActionResult MostChannels()
        {
            var dataResult = _homeService.GetMostChannels();
            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
        /// <summary>
        /// Gets the best comments
        /// </summary>
        /// <returns></returns>
        /// <response code="200">The best comments</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("mostcomment")]
        public IActionResult MostComment()
        {
            var dataResult = _homeService.GetMostComment();
            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoCardDto>>(dataResult.Data);
                mapResult.ForEach(dto =>
                {
                    dto.LikeCount = _countService.GetPhotoLikeCount(dto.PhotoId).Data;
                    dto.CommentCount = _countService.GetPhotoCommentCount(dto.PhotoId).Data;
                });

                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
        /// <summary>
        /// Gets the best photos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">The best photos</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("mostphotos")]
        public IActionResult MostPhotos()
        {
            var dataResult = _homeService.GetMostPhotos();
            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoCardDto>>(dataResult.Data);
                mapResult.ForEach(dto =>
                {
                    dto.LikeCount = _countService.GetPhotoLikeCount(dto.PhotoId).Data;
                    dto.CommentCount = _countService.GetPhotoCommentCount(dto.PhotoId).Data;
                });
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }

        /// <summary>
        /// Gets feed of the current user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Feed of the current user</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("feed")]
        [Authorize]
        public IActionResult Feed()
        {
            var userId = User.Claims.GetUserId();
            var dataResult = _homeService.GetFeed(userId.Data);
            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoCardDto>>(dataResult.Data);
                mapResult.ForEach(dto =>
                {
                    dto.LikeCount = _countService.GetPhotoLikeCount(dto.PhotoId).Data;
                    dto.CommentCount = _countService.GetPhotoCommentCount(dto.PhotoId).Data;
                });
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }

    }
}
