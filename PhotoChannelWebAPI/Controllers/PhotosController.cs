using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using CloudinaryDotNet.Actions;
using Core.Entities.Concrete;
using Core.Utilities.PhotoUpload;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;
using PhotoChannelWebAPI.Helpers;
using IResult = Core.Utilities.Results.IResult;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/photos")]
    [ApiController]
    [LogFilter]
    public class PhotosController : ControllerBase
    {
        private IPhotoService _photoService;
        private IMapper _mapper;
        private IPhotoUpload _photoUpload;
        private ICountService _countService;
        public PhotosController(IPhotoService photoService, IMapper mapper, IPhotoUpload photoUpload, ICountService countService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _photoUpload = photoUpload;
            _countService = countService;
        }
     
        /// <summary>
        /// Gets photo by id
        /// </summary>
        /// <param name="photoId">Photo's id</param>
        /// <response code="404">If the photo is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpGet("{photoId}")]
        public IActionResult Get(int photoId)
        {
            var contains = _photoService.Contains(new Photo { Id = photoId });
            if (contains)
            {
                IDataResult<Photo> dataResult = _photoService.GetById(photoId);

                if (dataResult.IsSuccessful)
                {
                    var mapResult = _mapper.Map<PhotoForDetailDto>(dataResult.Data);
                    this.CacheFill(mapResult);
                    return Ok(mapResult);
                }

                return this.ServerError(dataResult.Message);

            }

            return NotFound();
        }

        /// <summary>
        /// Gets channel photos by channel id
        /// </summary>
        /// <returns></returns>
        /// <param name="channelId">Channel id</param>
        /// <response code="404">If the channel is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Route("{channelId}/channel-photos")]
        public IActionResult GetChannelPhotos(int channelId)
        {
            IDataResult<List<Photo>> dataResult = _photoService.GetChannelPhotos(channelId);

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

            return this.ServerError(dataResult.Message);
        }
  
        /// <summary>
        /// Gets user photos by user id
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <response code="404">If the user is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}/user-photos")]
        public IActionResult GetUserPhotos(int userId)
        {
            IDataResult<List<Photo>> dataResult = _photoService.GetUserPhotos(userId);

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
            return this.ServerError(dataResult.Message);
        }

        /// <summary>
        /// Creates a photo
        /// </summary>
        /// <param name="photoForAddDto"></param>
        /// <response code="400">If the file length is less than zero.</response>
        /// <response code="404">If the channel is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ContainsFilter(typeof(IChannelService),typeof(Channel),nameof(PhotoForAddDto.ChannelId))]
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] PhotoForAddDto photoForAddDto)
        {
            if (photoForAddDto.File.Length > 0)
            {
                ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(photoForAddDto.File);
                var mapResult = _mapper.Map<Photo>(photoForAddDto);
                mapResult.PhotoUrl = imageUploadResult.Url.ToString();
                mapResult.PublicId = imageUploadResult.PublicId;
                mapResult.UserId = User.Claims.GetUserId().Data;
                IResult result = _photoService.Add(mapResult);

                if (result.IsSuccessful)
                {
                    this.RemoveCache();
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }
            return BadRequest();
        }
        /// <summary>
        /// Deletes a photo
        /// </summary>
        /// <returns></returns>
        /// <param name="photoId">Photo's id</param>
        /// <response code="404">If the photo is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IPhotoService), typeof(Photo))]
        [HttpDelete]
        [Route("{photoId}")]
        [Authorize]
        public IActionResult Delete(int photoId)
        {
            var deletedPhotos = _photoService.GetById(photoId);
            if (deletedPhotos.IsSuccessful)
            {
                IResult result = _photoService.Delete(deletedPhotos.Data);
                _photoUpload.ImageDelete(deletedPhotos.Data.PublicId);
                this.RemoveCache();
                return Ok(result.Message);
            }

            return BadRequest(deletedPhotos.Message);
        }

    }
}