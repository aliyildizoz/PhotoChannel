using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/subs")]
    [ApiController]
    public class SubsController : ControllerBase
    {
        private ISubscriberService _subscriberService;
        private IMapper _mapper;
        private IChannelService _channelService;
        public SubsController(ISubscriberService subscriberService, IMapper mapper, IChannelService channelService)
        {
            _subscriberService = subscriberService;
            _mapper = mapper;
            _channelService = channelService;
        }
        [HttpGet]
        [Route("{channelId}/subscribers")]
        public IActionResult GetSubscribers(int channelId)
        {
            IDataResult<List<User>> dataResult = _subscriberService.GetSubscribers(channelId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<SubscriberForListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpGet]
        [Route("issub/{channelId}")]
        [Authorize]
        public IActionResult GetIsSubs(int channelId)
        {
            return Ok(_subscriberService.GetIsUserSubs(channelId, User.Claims.GetUserId().Data));
        }
        [HttpGet]
        [Route("{userId}/subscriptions")]
        public IActionResult GetSubscriptions(int userId)
        {
            IDataResult<List<Channel>> dataResult = _subscriberService.GetSubscriptions(userId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                return Ok(mapResult);
            }

            return BadRequest(dataResult.Message);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] int channelId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful && channelId > 0)
            {
                return BadRequest();
            }
            IDataResult<Subscriber> dataResult = _subscriberService.Add(new Subscriber { UserId = resultId.Data, ChannelId = channelId });
            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }

        [HttpDelete]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Delete(int channelId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful && channelId > 0)
            {
                return BadRequest();
            }
            var result = _subscriberService.Delete(new Subscriber { UserId = resultId.Data, ChannelId = channelId });

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{channelId}/byowner/{userId}")]
        [Authorize]
        public IActionResult Delete(int channelId, int userId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful && userId > 0 && channelId > 0)
            {
                return BadRequest();
            }
            var isOwnerResult = _channelService.GetIsOwner(channelId, resultId.Data);
            if (isOwnerResult.IsSuccessful)
            {
                var result = _subscriberService.Delete(new Subscriber { ChannelId = channelId, UserId = userId });
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }

            return Forbid();
        }
    }
}