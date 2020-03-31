using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoChannelWebAPI.Dtos;

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

        [HttpPost]
        public IActionResult Post(LikeForAddDto likeDto)
        {
            var like = _mapper.Map<Like>(likeDto);
            IDataResult<Like> dataResult = _likeService.Add(like);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpDelete]
        public IActionResult Delete(LikeForDeleteDto likeDto)
        {
            var like = _mapper.Map<Like>(likeDto);
            IResult result = _likeService.Delete(like);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

    }
}