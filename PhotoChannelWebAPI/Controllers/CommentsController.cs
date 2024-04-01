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
using Microsoft.Extensions.DependencyInjection;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;
using PhotoChannelWebAPI.Helpers;
using IResult = Core.Utilities.Results.IResult;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [LogFilter]
    public class CommentsController : ControllerBase
    {
        private ICommentService _commentService;
        private IMapper _mapper;
        private ICountService _countService;

        public CommentsController(ICommentService commentService, IMapper mapper, ICountService countService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _countService = countService;
        }

        /// <summary>
        /// Gets photos of user did comment by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="404">If the user is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}/user-comment-photos")]
        public IActionResult GetPhotosByUserComment(int userId)
        {
            IDataResult<List<Photo>> dataResult = _commentService.GetPhotosByUserComment(userId);

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
        /// <summary>
        /// Gets users who commented on the photo by photo id
        /// </summary>
        /// <param name="photoId"></param>
        /// <response code="404">If the photo is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpGet]
        [Route("{photoId}/photo-comment-users")]
        public IActionResult GetUsersByPhotoComment(int photoId)
        {
            IDataResult<List<User>> result = _commentService.GetUsersByPhotoComment(photoId);
            var mapResult = _mapper.Map<List<UserForDetailDto>>(result.Data);
            if (mapResult.Count > 0)
            {
                this.CacheFill(mapResult);
            }
            return Ok(mapResult);
        }
        /// <summary>
        /// Gets photo comments by photo id
        /// </summary>
        /// <param name="photoId"></param>
        /// <response code="404">If the photo is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpGet]
        [Route("{photoId}/photo-comments")]
        public IActionResult GetPhotoComments(int photoId)
        {
            IDataResult<List<Comment>> result = _commentService.GetPhotoComments(photoId);

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<CommentForListDto>>(result.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult.OrderByDescending(dto => dto.ShareDate).ToList());
                }
                return Ok(mapResult.OrderByDescending(dto => dto.ShareDate).ToList());
            }

            return BadRequest(result.Message);
        }
        /// <summary>
        /// Creates a comment 
        /// </summary>
        /// <param name="commentForAddDto"></param>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the photo is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ContainsFilter(typeof(IPhotoService), typeof(Photo), nameof(CommentForAddDto.PhotoId))]
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
                    UserId = mapResult.UserId.GetValueOrDefault(),
                    LastName = currentUser.LastName,
                    FirstName = currentUser.FirstName
                };
                var channelId = HttpContext.RequestServices.GetService<IPhotoService>().GetById(commentForAddDto.PhotoId).Data.ChannelId;
                this.RemoveCacheByContains(currentUser.Id + "/user-photos");
                this.RemoveCacheByContains(currentUser.Id + "/user-comment-photos");
                this.RemoveCacheByContains(currentUser.Id + "/like-photos");
                this.RemoveCacheByContains(channelId + "/channel-photos");
                this.RemoveCache();
                return Ok(dto);
            }

            return BadRequest(result.Message);
        }
        /// <summary>
        /// Updates a comment 
        /// </summary>
        /// <param name="commentForUpdateDto"></param>
        /// <param name="commentId">Comment's id</param>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the photo is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Authorize]
        [Route("{commentId}")]
        public IActionResult Put(int commentId, CommentForUpdateDto commentForUpdateDto)
        {
            var comment = _commentService.GetById(commentId);
            if (comment.IsSuccessful)
            {
                comment.Data.Description = commentForUpdateDto.Description;
                IResult result = _commentService.Update(comment.Data);
                if (result.IsSuccessful)
                {
                    var channelId = HttpContext.RequestServices.GetService<IPhotoService>().GetById(comment.Data.PhotoId.GetValueOrDefault()).Data.ChannelId;
                    this.RemoveCacheByContains(comment.Data.UserId + "/user-photos");
                    this.RemoveCacheByContains(comment.Data.UserId + "/user-comment-photos");
                    this.RemoveCacheByContains(comment.Data.UserId + "/like-photos");
                    this.RemoveCacheByContains(channelId + "/channel-photos");
                    this.RemoveCache();
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes a comment
        /// </summary>
        /// <param name="commentId">Comment's id</param>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the comment is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Authorize]
        [Route("{commentId}")]
        public IActionResult Delete(int commentId)
        {
            var comment = _commentService.GetById(commentId);
            if (comment.IsSuccessful)
            {
                IResult result = _commentService.Delete(comment.Data);
                if (result.IsSuccessful)
                {
                    var channelId = HttpContext.RequestServices.GetService<IPhotoService>()
                        .GetById(comment.Data.PhotoId.GetValueOrDefault()).Data.ChannelId;
                    this.RemoveCacheByContains(comment.Data.UserId + "/user-photos");
                    this.RemoveCacheByContains(comment.Data.UserId + "/user-comment-photos");
                    this.RemoveCacheByContains(comment.Data.UserId + "/like-photos");
                    this.RemoveCacheByContains(channelId + "/channel-photos");
                    this.RemoveCache();
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }

            return NotFound(comment.Message);
        }
    }
}