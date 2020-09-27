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

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private ILikeService _likeService;
        private IMapper _mapper;
        public LikesController(ILikeService likeService, IMapper mapper)
        {
            _likeService = likeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{photoId}/photo-likes")]
        public IActionResult GetPhotoLikes(int photoId)
        {
            IDataResult<List<User>> dataResult = _likeService.GetPhotoLikes(photoId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<LikeForUserListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpGet]
        [Route("{photoId}/like-photos")]
        public IActionResult GetLikePhotos(int userId)
        {
            IDataResult<List<Photo>> dataResult = _likeService.GetLikePhotos(userId);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpGet]
        [Route("islike/{photoId}")]
        [Authorize]
        public IActionResult GetIsLike(int photoId)
        {
            return Ok(_likeService.GetIsUserLike(photoId, User.Claims.GetUserId().Data));
        }
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

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }

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
            var result = _likeService.Delete(new Like() { UserId = resultId.Data, PhotoId = photoId });

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

    }
}