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
        public IActionResult Post(SubscriberForAddDto subscriberDto)
        {
            var subscriber = _mapper.Map<Subscriber>(subscriberDto);
            IDataResult<Subscriber> dataResult = _subscriberService.Add(subscriber);

            if (dataResult.IsSuccessful)
            {
                return Ok(dataResult.Data);
            }

            return BadRequest(dataResult.Message);
        }
        [HttpDelete]
        public IActionResult Delete(SubscriberForDeleteDto subscriberDto)
        {
            var subscriber = _mapper.Map<Subscriber>(subscriberDto);
            IResult result = _subscriberService.Delete(subscriber);

            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}