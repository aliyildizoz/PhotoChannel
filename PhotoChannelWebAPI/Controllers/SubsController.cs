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
        public SubsController(ISubscriberService subscriberService, IMapper mapper)
        {
            _subscriberService = subscriberService;
            _mapper = mapper;
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
        public IActionResult Post(int channelId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful)
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
        [Authorize]
        public IActionResult Delete(int channelId)
        {
            var resultId = User.Claims.GetUserId();
            if (!resultId.IsSuccessful)
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
    }
}