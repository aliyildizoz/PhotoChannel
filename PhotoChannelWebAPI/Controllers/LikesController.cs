using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/likes")]
    [ApiController]
    [LogFilter]
    public class LikesController : ControllerBase
    {
        private ILikeService _likeService;
        private IMapper _mapper;
        private IPhotoService _photoService;
        private ICountService _countService;

        public LikesController(ICountService countService, ILikeService likeService, IMapper mapper, IPhotoService photoService)
        {
            _likeService = likeService;
            _mapper = mapper;
            _photoService = photoService;
            _countService = countService;
        }

        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpGet]
        [Route("{photoId}/photo-likes")]
        public IActionResult GetPhotoLikes(int photoId)
        {
            IDataResult<List<User>> dataResult = _likeService.GetPhotoLikes(photoId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<LikeForUserListDto>>(dataResult.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}/like-photos")]
        public IActionResult GetLikePhotos(int userId)
        {
            IDataResult<List<Photo>> dataResult = _likeService.GetLikePhotos(userId);

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
                    this.CacheFill(mapResult.OrderByDescending(dto => dto.ShareDate).ToList());
                }
                return Ok(mapResult.OrderByDescending(dto => dto.ShareDate).ToList());
            }

            return BadRequest(dataResult.Message);
        }
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpGet]
        [Route("islike/{photoId}")]
        [Authorize]
        public IActionResult GetIsLike(int photoId)
        {
            var result = _likeService.GetIsUserLike(photoId, User.Claims.GetUserId().Data);
            this.CacheFillWithUserId(result);
            return Ok(result);
        }
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] int photoId)
        {
            var result = User.Claims.GetUserId();
            if (!result.IsSuccessful && photoId > 0)
            {
                return BadRequest();
            }
            IDataResult<Like> dataResult = _likeService.Add(new Like() { UserId = result.Data, PhotoId = photoId });
            var channelId = _photoService.GetById(photoId).Data.ChannelId;

            if (dataResult.IsSuccessful)
            {

                this.RemoveCacheByContains(result.Data + "/api/likes/islike/" + photoId);
                this.RemoveCacheByContains("photos/" + photoId);
                this.RemoveCacheByContains(channelId + "/channel-photos");
                this.RemoveCacheByContains(result.Data + "/user-photos");
                this.RemoveCacheByContains(result.Data + "/like-photos");
                this.RemoveCacheByContains(result.Data + "/user-comment-photos");
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpDelete]
        [Route("{photoId}")]
        [Authorize]
        public IActionResult Delete(int photoId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful && photoId > 0)
            {
                return BadRequest();
            }

            var channelId = _photoService.GetById(photoId).Data.ChannelId;
            var result = _likeService.Delete(new Like() { UserId = resultId.Data, PhotoId = photoId });

            if (result.IsSuccessful)
            {
                this.RemoveCacheByContains(resultId.Data + "/api/likes/islike/" + photoId);
                this.RemoveCacheByContains("photos/" + photoId);
                this.RemoveCacheByContains(channelId + "/channel-photos");
                this.RemoveCacheByContains(resultId.Data + "/user-photos");
                this.RemoveCacheByContains(resultId.Data + "/like-photos");
                this.RemoveCacheByContains(resultId.Data + "/user-comment-photos");
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

    }
}