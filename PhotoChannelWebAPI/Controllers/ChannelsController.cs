using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using CloudinaryDotNet.Actions;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.PhotoUpload;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    [LogFilter]
    public class ChannelsController : ControllerBase
    {
        private IChannelService _channelService;
        private IMapper _mapper;
        private IPhotoUpload _photoUpload;
        private ICountService _countService;
        public ChannelsController(IChannelService channelService, IMapper mapper, IPhotoUpload photoUpload, ICountService countService)
        {
            _channelService = channelService;
            _mapper = mapper;
            _photoUpload = photoUpload;
            _countService = countService;
        }
        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///    POST /Channel
        ///    {
        ///       "name": 1,
        ///       "file": []
        ///    }
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <param name="channelForAddDto"></param>
        /// <response code="200">A newly created channel.</response>
        /// <response code="400">If the channel name already exists.</response>
        /// <response code="400">If there is no channel photo.</response>
        /// <response code="400">If the channel couldn't be added</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] ChannelForAddDto channelForAddDto)
        {
            var resultId = User.Claims.GetUserId();
            IResult checkResult = _channelService.CheckIfChannelNameExists(channelForAddDto.Name);
            if (!checkResult.IsSuccessful)
            {
                return BadRequest(checkResult.Message);
            }

            if (channelForAddDto.File.Length > 0)
            {
                ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForAddDto.File);
                if ((int)imageUploadResult.StatusCode < 300)
                {
                    var channel = _mapper.Map<Channel>(channelForAddDto);
                    channel.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                    channel.PublicId = imageUploadResult.PublicId;
                    channel.UserId = resultId.Data;
                    IResult result = _channelService.Add(channel);
                    if (result.IsSuccessful)
                    {
                        var mapResult = _mapper.Map<ChannelForDetailDto>(channel);
                        this.RemoveCacheByContains(resultId.Data + "/user-channels");
                        return Ok(mapResult);
                    }
                    return BadRequest(result.Message);
                }
                return BadRequest(imageUploadResult.Error.Message);
            }
            return BadRequest();
        }
        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///    POST /Channel
        ///    {
        ///       "name": 1,
        ///       "file": []
        ///    }
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <param name="channelForAddDto"></param>
        /// <response code="200">A newly created channel.</response>
        /// <response code="400">If the channel name already exists.</response>
        /// <response code="400">If there is no channel photo.</response>
        /// <response code="400">If the channel couldn't be added</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpDelete]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Delete(int channelId)
        {
            IResult isOwner = _channelService.GetIsOwner(channelId, User.Claims.GetUserId().Data);
            if (isOwner.IsSuccessful)
            {

                IDataResult<Channel> dataResult = _channelService.GetById(channelId);
                IResult result = _channelService.Delete(channelId);
                if (result.IsSuccessful)
                {
                    _photoUpload.ImageDelete(dataResult.Data.PublicId);
                    this.RemoveCache();
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);

            }

            return BadRequest(isOwner.Message);
        }

        [HttpPut]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Put(int channelId, [FromForm] ChannelForUpdateDto channelForUpdate)
        {
            var dataResult = _channelService.GetById(channelId);
            if (dataResult.IsSuccessful)
            {
                if (string.IsNullOrEmpty(channelForUpdate.Name))
                {
                    return BadRequest();
                }
                if (channelForUpdate.File != null && channelForUpdate.File.Length > 0 && BusinessRules.ImageExtensionValidate(channelForUpdate.File.ContentType).IsSuccessful)
                {
                    DeletionResult deletionResult = _photoUpload.ImageDelete(dataResult.Data.PublicId);
                    ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForUpdate.File);
                    dataResult.Data.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                    dataResult.Data.PublicId = imageUploadResult.PublicId;
                }
                IResult result = _channelService.CheckIfChannelNameExistsWithUpdate(channelForUpdate.Name, channelId);
                if (result.IsSuccessful)
                {
                    return BadRequest(result.Message);
                }
                dataResult.Data.Name = channelForUpdate.Name;

                IDataResult<Channel> updateDataResult = _channelService.Update(dataResult.Data);
                if (dataResult.IsSuccessful)
                {
                    var map = _mapper.Map<ChannelForDetailDto>(updateDataResult.Data);
                    map.SubscribersCount = _countService.GetSubscriberCount(map.Id).Data;
                    this.RemoveCache();
                    return Ok(map);
                }

                return this.ServerError();
            }
            return NotFound(dataResult.Message);
        }


        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            IDataResult<List<Channel>> result = _channelService.GetList();
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("{channelName}")]
        public IActionResult GetChannelByName(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                IDataResult<List<Channel>> result = _channelService.GetByName(channelName);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                    if (mapResult.Count > 0)
                    {
                        this.CacheFill(mapResult);
                    }
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}")]
        public IActionResult GetById(int channelId)
        {
            IDataResult<Channel> result = _channelService.GetById(channelId);
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<ChannelForDetailDto>(result.Data);
                mapResult.SubscribersCount = _countService.GetSubscriberCount(channelId).Data;
                this.CacheFill(mapResult);
                return Ok(mapResult);
            }

            return NotFound(result.Message);
        }
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}/user-channels")]
        public IActionResult GetUserChannels(int userId)
        {
            var result = _channelService.GetUserChannels(userId);
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }
            return this.ServerError(result.Message);
        }
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Route("{channelId}/owner")]
        public IActionResult GetOwner(int channelId)
        {
            IDataResult<User> result = _channelService.GetOwner(channelId);
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                this.CacheFill(mapResult);
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Authorize]
        [Route("{channelId}/isowner")]
        public IActionResult GetChannelIsOwner(int channelId)
        {
            IResult result = _channelService.GetIsOwner(channelId, User.Claims.GetUserId().Data);
            this.CacheFill(result.IsSuccessful);
            return Ok(result.IsSuccessful);
        }
    }
}