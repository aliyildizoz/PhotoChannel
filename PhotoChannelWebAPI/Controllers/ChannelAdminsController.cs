using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channeladmins")]
    [ApiController]
    public class ChannelAdminsController : ControllerBase
    {
        private IChannelAdminsService _channelAdminsService;
        private IMapper _mapper;
        public ChannelAdminsController(IChannelAdminsService channelAdminsService, IMapper mapper)
        {
            _channelAdminsService = channelAdminsService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("{channelId}/channel-admins")]
        public IActionResult GetChannelAdmins(int channelId)
        {
            IDataResult<List<User>> dataResult = _channelAdminsService.GetChannelAdmins(channelId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForAdminListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpGet]
        [Route("{userId}/like-photos")]
        public IActionResult GetAdminChannels(int userId)
        {
            IDataResult<List<Channel>> dataResult = _channelAdminsService.GetAdminChannels(userId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpPost]
        public IActionResult Post(ChannelAdminForAddDto channelAdminDto)
        {
            var channelAdmin = _mapper.Map<ChannelAdmin>(channelAdminDto);
            IDataResult<ChannelAdmin> dataResult = _channelAdminsService.Add(channelAdmin);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpDelete]
        public IActionResult Delete(ChannelAdminForDeleteDto channelAdminDto)
        {
            var channelAdmin = _mapper.Map<ChannelAdmin>(channelAdminDto);
            IResult result = _channelAdminsService.Delete(channelAdmin);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}