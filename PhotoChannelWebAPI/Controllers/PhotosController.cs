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

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IPhotoService _photoService;
        private IMapper _mapper;
        private IPhotoUpload _photoUpload;
        public PhotosController(IPhotoService photoService, IMapper mapper, IPhotoUpload photoUpload)
        {
            _photoService = photoService;
            _mapper = mapper;
            _photoUpload = photoUpload;
        }
        [HttpGet("{photoId}")]
        public IActionResult Get(int photoId)
        {
            IDataResult<Photo> result = _photoService.GetById(photoId);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("{photoId}/likes")]
        public IActionResult GetLikeUsersByPhoto(int photoId)
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
        public IActionResult Post(PhotoForAddDto photoForAddDto)
        {
            if (photoForAddDto.File.Length > 0)
            {
                ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(photoForAddDto.File);
                var mapResult = _mapper.Map<Photo>(photoForAddDto);
                mapResult.PhotoUrl = imageUploadResult.Uri.ToString();
                mapResult.PublicId = imageUploadResult.PublicId;
                IResult result = _photoService.Add(mapResult);

                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }


            return BadRequest();
        }
        [HttpDelete("{photoId}")]
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
        public IActionResult DeleteLike(Like like)
        {
            IResult result = _photoService.DeleteLike(like);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        public IActionResult Post(Like like)
        {
            IResult result = _photoService.AddLike(like);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}