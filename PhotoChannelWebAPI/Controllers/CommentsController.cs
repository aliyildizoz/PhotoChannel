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
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService _commentService;
        private IMapper _mapper;

        public CommentsController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
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
                var mapResult = _mapper.Map<List<CommentForListDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Post(CommentForAddDto commentForAddDto)
        {
            var currentUser = User.Claims.GetCurrentUser().Data;
            var mapResult = _mapper.Map<Comment>(commentForAddDto);
            mapResult.UserId = currentUser.Id;
            IResult result = _commentService.Add(mapResult);
            if (result.IsSuccessful)
            {
                CommentForListDto dto = new CommentForListDto
                {
                    CommentId = mapResult.Id,
                    Description = mapResult.Description,
                    ShareDate = mapResult.ShareDate,
                    UserId = mapResult.UserId,
                    LastName = currentUser.LastName,
                    FirstName = currentUser.FirstName
                };
                return Ok(dto);
            }

            return BadRequest(result.Message);
        }
        [HttpPut]
        [Authorize]
        [Route("{commentId}")]
        public IActionResult Put(int commentId, CommentForUpdateDto commentForUpdateDto)
        {
            if (commentId > 0)
            {
                var comment = _commentService.GetById(commentId);
                if (comment.IsSuccessful)
                {
                    comment.Data.Description = commentForUpdateDto.Description;
                    IResult result = _commentService.Update(comment.Data);
                    if (result.IsSuccessful)
                    {
                        return Ok(result.Message);
                    }
                    return this.ServerError(result.Message);
                }

                return NotFound();

            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        [Route("{commentId}")]
        public IActionResult Delete(int commentId)
        {
            if (commentId > 0)
            {
                IResult result = _commentService.Delete(new Comment { Id = commentId });

                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }
            return BadRequest();
        }
    }
}