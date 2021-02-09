using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/home")]
    [ApiController]
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
        [HttpGet("mostchannels")]
        public IActionResult MostChannels()
        {
            var dataResult = _homeService.GetMostChannels();
            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
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
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
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
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
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
