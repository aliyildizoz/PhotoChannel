using System;
using System.Collections.Generic;
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

        [HttpPost("add")]
        public IActionResult Add(Channel channel)
        {

            IResult result = _channelService.Add(channel);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpPost("update")]
        public IActionResult Update(Channel channel)
        {
            if (channel.Id > 0)
            {
                IResult result = _channelService.Add(channel);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }

            return BadRequest();
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            IDataResult<List<Channel>> result = _channelService.GetList();
            if (result.IsSuccessful)
            {
                var resultForMap = _mapper.Map<List<ChannelForListDto>>(result.Data);
                return Ok(resultForMap);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("getbyname")]
        public IActionResult GetByName(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                IDataResult<List<Channel>> result = _channelService.GetByName(channelName);
                if (result.IsSuccessful)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int channelId)
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
        [HttpGet("getadminlist")]
        public IActionResult GetAdminList(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetAdminList(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
        [HttpGet("getsubscribers")]
        public IActionResult GetSubscribers(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<List<User>> result = _channelService.GetSubscribers(new Channel { Id = channelId });
                if (result.IsSuccessful)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }
            return BadRequest();
        }
    }
}