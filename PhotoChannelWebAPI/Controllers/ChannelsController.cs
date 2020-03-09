using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using CloudinaryDotNet.Actions;
using Core.Entities.Concrete;
using Core.Utilities.PhotoUpload;
using Core.Utilities.Results;
using Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private IChannelService _channelService;
        private IMapper _mapper;
        private IPhotoUpload _photoUpload;
        private IAuthHelper _authHelper;
        public ChannelsController(IChannelService channelService, IMapper mapper, IPhotoUpload photoUpload, IAuthHelper authHelper)
        {
            _channelService = channelService;
            _mapper = mapper;
            _photoUpload = photoUpload;
            _authHelper = authHelper;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Add([FromForm]ChannelForAddDto channelForAddDto)
        {
            string ownerId = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(ownerId))
            {
                if (channelForAddDto.File.Length > 0)
                {
                    ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForAddDto.File);
                    var channel = _mapper.Map<Channel>(channelForAddDto);
                    channel.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                    channel.PublicId = imageUploadResult.PublicId;
                    channel.OwnerId = int.Parse(ownerId);
                    IResult result = _channelService.Add(channel);
                    if (result.IsSuccessful)
                    {
                        var mapResult = _mapper.Map<ChannelForDetailDto>(channel);
                        return Ok(mapResult);
                    }

                    return BadRequest(result.Message);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("subscribe")]
        public IActionResult Subscribe(int channelId)
        {
            string id = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(id) && channelId > 0)
            {
                IResult result = _channelService.AddSubscribe(new Subscriber { ChannelId = channelId, UserId = int.Parse(id) });
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("addcategory")]
        public IActionResult AddCategory(ChannelCategory channelCategory)
        {
            IResult result = _channelService.AddChannelCategory(channelCategory);
            if (result.IsSuccessful)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        [Route("addchanneladmin")]
        public IActionResult AddChannelAdmin(ChannelAdmin channelAdmin)
        {
            IResult result = _channelService.AddChannelAdmin(channelAdmin);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{channelId}")]
        public IActionResult Delete(int channelId)
        {
            if (channelId > 0)
            {
                IResult result = _channelService.Delete(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }


            return BadRequest();
        }
        [HttpDelete]
        [Route("deletechanneladmin")]
        public IActionResult DeleteChannelAdmin(ChannelAdmin channelAdmin)
        {
            IResult result = _channelService.DeleteChannelAdmin(channelAdmin);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpDelete]
        [Route("{channelId}/unsubscribe")]
        public IActionResult Unsubscribe(int channelId)
        {
            string id = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(id) && channelId > 0)
            {
                IResult result = _channelService.DeleteSubscribe(new Subscriber { ChannelId = channelId, UserId = int.Parse(id) });
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("deletechannelcategory")]
        public IActionResult DeleteChannelCategory(ChannelCategory channelCategory)
        {
            IResult result = _channelService.DeleteChannelCategory(channelCategory);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Put(ChannelForUpdateDto channelForUpdate)
        {
            if (channelForUpdate.Id > 0)
            {
                if (channelForUpdate.File.Length > 0)
                {
                    ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForUpdate.File);
                    var mapResult = _mapper.Map<Channel>(channelForUpdate);
                    mapResult.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                    mapResult.PublicId = imageUploadResult.PublicId;
                    IResult result = _channelService.Update(mapResult);
                    if (result.IsSuccessful)
                    {
                        return Ok(result.Message);
                    }

                    return BadRequest(result.Message);
                }

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Get()
        {
            IDataResult<List<Channel>> result = _channelService.GetList();
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpGet]//böylemi yazılıyor bak
        [Route("{channelName}")]
        public IActionResult GetChannelByName(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                IDataResult<List<Channel>> result = _channelService.GetByName(channelName);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}")]
        public IActionResult GetChannelById(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<Channel> result = _channelService.GetById(channelId);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<ChannelForDetailDto>(result.Data);
                    return Ok(mapResult);
                }

                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}/admin-list")]
        public IActionResult GetAdminList(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetAdminList(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<ChannelForAdminListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}/subscribers")]
        public IActionResult GetSubscribers(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetSubscribers(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<SubscriberForListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}/owner")]
        public IActionResult GetOwner(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<User> result = _channelService.GetOwner(channelId);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}/photos")]
        public IActionResult GetPhotos(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<Photo>> result = _channelService.GetPhotos(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<PhotoForListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
    }
}