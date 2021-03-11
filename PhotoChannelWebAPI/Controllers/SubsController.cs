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
        /// <summary>
        /// Gets subscribers of the channel by channel id
        /// </summary>
        /// <param name="channelId">Channel's id</param>
        /// <response code="404">If the channel not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// "Does the current user subscribe to this channel?" gets response by the channel's identity
        /// </summary>
        /// <param name="channelId">Channel's id</param>
        /// <response code="400">If current user not owner of the channel</response>
        /// <response code="401">If the user is unauthorize</response>  
        /// <response code="404">If the channel not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Gets subscriptions of the user by user id
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <response code="404">If the user not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Subscribes current user by channel id
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the channel is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] int channelId)
        {
            IDataResult<Subscriber> dataResult = _subscriberService.Add(new Subscriber { UserId = User.Claims.GetUserId().Data, ChannelId = channelId });
            if (dataResult.IsSuccessful)
            {
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/subscriptions");
                this.RemoveCacheByContains(channelId + "/subscribers");
                this.RemoveCacheByContains(User.Claims.GetUserId().Data + "/api/subs/issub/" + channelId);
                this.RemoveCacheByContains("/api/channels/" + channelId);
                return Ok(dataResult.Data);
            }

            return this.ServerError(dataResult.Message);
        }


        /// <summary>
        /// Unsubscribes current user by channel id
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <response code="401">If the user is unauthorize</response>
        /// <response code="404">If the channel is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                this.RemoveCacheByContains("/api/channels/" + channelId);
                return Ok(result.Message);
            }

            return this.ServerError(result.Message);
        }

        /// <summary>
        /// Channel owner unsubscribes by to user id
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User's id</param>
        /// <response code="401">If the current user is unauthorize</response>
        /// <response code="403">If the current user isn't owner of the channel</response>
        /// <response code="404">If the channel is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ContainsFilter(typeof(IChannelService), typeof(Channel))]
        [HttpDelete]
        [Route("{channelId}/byowner/{userId}")]
        [Authorize]
        public IActionResult Delete(int channelId, int userId)
        {
            if (userId < 0)
            {
                return BadRequest();
            }
            var isOwnerResult = _channelService.GetIsOwner(channelId, User.Claims.GetUserId().Data);
            if (isOwnerResult.IsSuccessful)
            {
                var result = _subscriberService.Delete(new Subscriber { ChannelId = channelId, UserId = userId });
                if (result.IsSuccessful)
                {
                    this.RemoveCacheByContains(userId + "/subscriptions");
                    this.RemoveCacheByContains(channelId + "/subscribers");
                    this.RemoveCacheByContains(userId + "/api/subs/issub/" + channelId);
                    return Ok(result.Message);
                }

                return this.ServerError(result.Message);

            }

            return Forbid();
        }
    }
}