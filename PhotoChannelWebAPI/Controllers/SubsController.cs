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
using PhotoChannelWebAPI.Filters;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/subs")]
    [ApiController]
    [LogFilter]
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
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Route("{channelId}/subscribers")]
        public IActionResult GetSubscribers(int channelId)
        {
            IDataResult<List<User>> dataResult = _subscriberService.GetSubscribers(channelId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<SubscriberForListDto>>(dataResult.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpGet]
        [Route("issub/{channelId}")]
        [Authorize]
        public IActionResult GetIsSubs(int channelId)
        {
            var res = _subscriberService.GetIsUserSubs(channelId, User.Claims.GetUserId().Data);
            this.CacheFillWithUserId(res);
            return Ok(res);
        }
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}/subscriptions")]
        public IActionResult GetSubscriptions(int userId)
        {
            IDataResult<List<Channel>> dataResult = _subscriberService.GetSubscriptions(userId);

            if (dataResult.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(dataResult.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }

            return this.ServerError(dataResult.Message);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] int channelId)
        {
            //Todo: channelId var mı kontrolü 

            IDataResult<Subscriber> dataResult = _subscriberService.Add(new Subscriber { UserId = User.Claims.GetUserId().Data, ChannelId = channelId });
            if (dataResult.IsSuccessful)
            {
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/subscriptions");
                this.RemoveCacheByContains(channelId + "/subscribers");
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/api/subs/issub/" + channelId);
                return Ok(dataResult.Data);
            }

            return this.ServerError(dataResult.Message);
        }
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpDelete]
        [Route("{channelId}")]
        [Authorize]
        public IActionResult Delete(int channelId)
        {
            var result = _subscriberService.Delete(new Subscriber { UserId = User.Claims.GetUserId().Data, ChannelId = channelId });

            if (result.IsSuccessful)
            {
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/subscriptions");
                this.RemoveCacheByContains(channelId + "/subscribers");
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/api/subs/issub/" + channelId);

                return Ok(result.Message);
            }

            return this.ServerError(result.Message);
        }
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
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
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/subscriptions");
                this.RemoveCacheByContains(channelId + "/subscribers");
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/api/subs/issub/" + channelId);

                return this.ServerError(result.Message);

            }

            return Forbid();
        }
    }
}