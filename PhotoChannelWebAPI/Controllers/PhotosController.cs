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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
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
        private IAuthHelper _authHelper;
        public PhotosController(IPhotoService photoService, IMapper mapper, IPhotoUpload photoUpload, IAuthHelper authHelper)
        {
            _photoService = photoService;
            _mapper = mapper;
            _photoUpload = photoUpload;
            _authHelper = authHelper;
        }
        [HttpGet("{photoId}")]
        public IActionResult Get(int photoId)
        {
            IDataResult<Photo> dataResult = _photoService.GetById(photoId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<PhotoForDetailDto>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpGet]
        [Route("{channelId}/channel-photos")]
        public IActionResult GetChannelPhotos(int channelId)
        {
            IDataResult<List<Photo>> dataResult = _photoService.GetChannelPhotos(channelId);

            if (dataResult.IsSuccessful)
            {
                //Todo:Dto
                //var mapResult = _mapper.Map<PhotoForDetailDto>(dataResult.Data);
                return Ok();
            }

            return BadRequest(dataResult.Message);
        }
        [HttpGet]
        [Route("{userId}/user-photos")]
        public IActionResult GetUserPhotos(int userId)
        {
            IDataResult<List<Photo>> dataResult = _photoService.GetUserPhotos(userId);

            if (dataResult.IsSuccessful)
            {
                //Todo:Dto
                //var mapResult = _mapper.Map<PhotoForDetailDto>(dataResult.Data);
                return Ok();
            }

            return BadRequest(dataResult.Message);
        }
        [HttpPost]
        public IActionResult Post([FromForm]PhotoForAddDto photoForAddDto)
        {
            string userId = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                if (photoForAddDto.File.Length > 0)
                {
                    ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(photoForAddDto.File);
                    var mapResult = _mapper.Map<Photo>(photoForAddDto);
                    mapResult.PhotoUrl = imageUploadResult.Uri.ToString();
                    mapResult.PublicId = imageUploadResult.PublicId;
                    mapResult.UserId = int.Parse(userId);
                    IResult result = _photoService.Add(mapResult);

                    if (result.IsSuccessful)
                    {
                        return Ok(result.Message);
                    }
                    return BadRequest(result.Message);
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("{photoId}")]
        public IActionResult Delete(int photoId)
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

    }
}