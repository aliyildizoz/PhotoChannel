using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private IChannelService _channelService;

        public ChannelsController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpPost("add")]
        public IActionResult Add(Channel channel)
        {
            IDataResult<Channel> result = _channelService.Add(channel);
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.IsSuccessful);
        }


        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            IDataResult<List<Channel>> result = _channelService.GetList();
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.IsSuccessful);
        }
    }
}