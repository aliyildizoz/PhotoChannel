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
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/photos")]
    [ApiController]
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
                    return Ok(mapResult);
                }

                return this.ServerError(dataResult.Message);

            }

            return NotFound();
        }
        [HttpGet]
        [Route("{channelId}/channel-photos")]
        public IActionResult GetChannelPhotos(int channelId)
        {
            //Todo: channelId var mı kontrolü 
            IDataResult<List<Photo>> dataResult = _photoService.GetChannelPhotos(channelId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoCardDto>>(dataResult.Data);
                mapResult.ForEach(dto =>
                {
                    dto.LikeCount = _countService.GetPhotoLikeCount(dto.PhotoId).Data;
                    dto.CommentCount = _countService.GetPhotoCommentCount(dto.PhotoId).Data;
                });
                return Ok(mapResult.OrderByDescending(dto => dto.ShareDate).ToList());
            }

            return this.ServerError(dataResult.Message);
        }
        [HttpGet]
        [Route("{userId}/user-photos")]
        public IActionResult GetUserPhotos(int userId)
        {
            //Todo: userId var mı kontrolü 

            IDataResult<List<Photo>> dataResult = _photoService.GetUserPhotos(userId);

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

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] PhotoForAddDto photoForAddDto)
        {
            if (photoForAddDto.File.Length > 0)
            {
                ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(photoForAddDto.File);
                var mapResult = _mapper.Map<Photo>(photoForAddDto);
                mapResult.PhotoUrl = imageUploadResult.Uri.ToString();
                mapResult.PublicId = imageUploadResult.PublicId;
                mapResult.UserId = User.Claims.GetUserId().Data;
                IResult result = _photoService.Add(mapResult);

                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("{photoId}")]
        [Authorize]
        public IActionResult Delete(int photoId)
        {
            var contains = _photoService.Contains(new Photo { Id = photoId });
            if (contains)
            {
                var deletedPhotos = _photoService.GetById(photoId);
                if (deletedPhotos.IsSuccessful)
                {
                    IResult result = _photoService.Delete(deletedPhotos.Data);
                    _photoUpload.ImageDelete(deletedPhotos.Data.PublicId);
                    return Ok(result.Message);
                }

                return BadRequest(deletedPhotos.Message);
            }

            return NotFound();
        }

    }
}