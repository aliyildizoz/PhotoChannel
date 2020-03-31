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
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService _commentService;
        private IMapper _mapper;
        private IAuthHelper _authHelper;

        public CommentsController(ICommentService commentService, IMapper mapper, IAuthHelper authHelper)
        {
            _commentService = commentService;
            _mapper = mapper;
            _authHelper = authHelper;
        }

        [HttpGet]
        [Route("{userId}/user-comment-photos")]
        public IActionResult GetPhotosByUserComment(int userId)
        {
            IDataResult<List<Photo>> result = _commentService.GetPhotosByUserComment(userId);

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoForDetailDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("{photoId}/photo-comment-users")]
        public IActionResult GetUsersByPhotoComment(int photoId)
        {
            IDataResult<List<User>> result = _commentService.GetUsersByPhotoComment(photoId);

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<UserForDetailDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("{photoId}/photo-comments")]
        public IActionResult GetPhotoComments(int photoId)
        {
            IDataResult<List<Comment>> result = _commentService.GetPhotoComments(photoId);

            if (result.IsSuccessful)
            {
                //Todo:Dto ile veriyi eşleştir
                var mapResult = _mapper.Map<List<CommentForListDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        public IActionResult Post(CommentForAddDto commentForAddDto)
        {
            string userId = _authHelper.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                var mapResult = _mapper.Map<Comment>(commentForAddDto);
                IResult result = _commentService.Add(mapResult);
                mapResult.UserId = int.Parse(userId);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }

            return BadRequest();
        }
        [HttpPut]
        public IActionResult Put(CommentForUpdateDto commentForUpdateDto)
        {
            string userId = _authHelper.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                var mapResult = _mapper.Map<Comment>(commentForUpdateDto);
                IResult result = _commentService.Update(mapResult);
                mapResult.UserId = int.Parse(userId);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }

            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int commentId)
        {
            IResult result = _commentService.Delete(new Comment { Id = commentId });

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}