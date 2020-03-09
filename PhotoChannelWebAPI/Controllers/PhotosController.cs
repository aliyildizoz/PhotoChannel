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
        [Route("{photoId}/likes")]
        public IActionResult GetLikes(int photoId)
        {
            IDataResult<List<User>> result = _photoService.GetLikeUsersByPhoto(new Photo { Id = photoId });

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<LikeForUserListDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromForm]PhotoForAddDto photoForAddDto)
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
            IResult result = _photoService.Delete(new Photo { Id = photoId });

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpDelete]
        [Route("{photoId}/deletelike")]
        public IActionResult DeleteLike(int photoId)
        {
            string userId = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                IResult result = _photoService.DeleteLike(new Like { PhotoId = photoId, UserId = int.Parse(userId) });

                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }


            return BadRequest();
        }
        [HttpPost]
        [Route("like")]
        public IActionResult AddLike(int photoId)
        {
            string userId = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                IResult result = _photoService.AddLike(new Like { PhotoId = photoId, UserId = int.Parse(userId) });

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