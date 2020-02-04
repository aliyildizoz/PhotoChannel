using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private IChannelService _channelService;
        private IMapper _mapper;
        public ChannelsController(IChannelService channelService, IMapper mapper)
        {
            _channelService = channelService;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Post(ChannelForAddDto channelForAddDto)
        {
            var mapResult = _mapper.Map<Channel>(channelForAddDto);
            IResult result = _channelService.Add(mapResult);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpPost]
        public IActionResult Post(Subscriber subscriber)
        {
            IResult result = _channelService.AddSubscribe(subscriber);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Post(ChannelAdmin channelAdmin)
        {
            IResult result = _channelService.AddChannelAdmin(channelAdmin);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete("{channelId}")]
        public IActionResult Delete(int channelId)
        {
            IResult result = _channelService.Delete(new Channel { Id = channelId });
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpDelete]
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
        public IActionResult DeleteSubscribe(Subscriber subscriber)
        {
            IResult result = _channelService.DeleteSubscribe(subscriber);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpPut("")]
        public IActionResult Put(ChannelForUpdateDto channelForUpdate)
        {
            if (channelForUpdate.Id > 0)
            {
                var mapResult = _mapper.Map<Channel>(channelForUpdate);
                IResult result = _channelService.Update(mapResult);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }

            return BadRequest();
        }

        [HttpGet("")]
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
        [HttpGet("{channelName}")]
        public IActionResult Get(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                IDataResult<List<Channel>> result = _channelService.GetByName(channelName);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("{channelId}")]
        public IActionResult Get(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<Channel> result = _channelService.GetById(channelId);
                if (result.IsSuccessful)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("{channelId}/admin-list")]
        public IActionResult GetAdminList(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetAdminList(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<ChannelForAdminListDto>(result.Data);
                    return Ok(mapResult);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("{channelId}/subscribers")]
        public IActionResult GetSubscribers(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetSubscribers(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<SubscriberForListDto>(result.Data);
                    return Ok(mapResult);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("{channelId}/owner")]
        public IActionResult GetOwner(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<User> result = _channelService.GetOwner(channelId);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<ChannelForOwnerDto>(result.Data);
                    return Ok(mapResult);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("{channelId}/photos")]
        public IActionResult GetPhotos(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<Photo>> result = _channelService.GetPhotos(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<PhotoForListDto>(result.Data);
                    return Ok(mapResult);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
    }
}