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

            return this.ServerError(dataResult.Message);
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

            return this.ServerError(dataResult.Message);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm]int channelId)
        {
            //Todo: channelId var mı kontrolü 

            IDataResult<Subscriber> dataResult = _subscriberService.Add(new Subscriber { UserId = User.Claims.GetUserId().Data, ChannelId = channelId });
            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return this.ServerError(dataResult.Message);
        }

        [HttpDelete]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Delete(int channelId)
        {
            var result = _subscriberService.Delete(new Subscriber { UserId = User.Claims.GetUserId().Data, ChannelId = channelId });

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return this.ServerError(result.Message);
        }

        [HttpDelete]
        [Route("{channelId}/byowner/{userId}")]
        [Authorize]
        public IActionResult Delete(int channelId, int userId)
        {
            if (userId > 0 && channelId > 0)
            {
                return BadRequest();
            }
            var isOwnerResult = _channelService.GetIsOwner(channelId, User.Claims.GetUserId().Data);
            if (isOwnerResult.IsSuccessful)
            {
                var result = _subscriberService.Delete(new Subscriber { ChannelId = channelId, UserId = userId });
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);

            }

            return Forbid();
        }
    }
}