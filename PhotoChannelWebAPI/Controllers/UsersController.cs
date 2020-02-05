using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet("")]
        public IActionResult Get()
        {
            IDataResult<List<User>> result = _userService.GetList();

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<UserForListDto>>(result.Data);
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            IDataResult<User> result1 = _userService.GetById(userId);
            IDataResult<UserDetail> result2 = _userService.GetUserDetailById(userId);

            if (result1.IsSuccessful && result2.IsSuccessful)
            {
                var mapResult = _mapper.Map<UserForDetailDto>(result1.Data);
                mapResult.SubscriptionCount = result2.Data.SubscriptionCount;
                return Ok(mapResult);
            }
            return BadRequest(result1.Message + " " + result1.Message);
        }
        [HttpGet("{userId}/photos")]
        public IActionResult GetPhotos(int userId)
        {
            IDataResult<List<Photo>> result = _userService.GetPhotos(new User { Id = userId });
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoForListDto>>(result.Data);
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("{userId}/subscriptions")]
        public IActionResult GetSubscriptions(int userId)
        {
            IDataResult<List<Channel>> result = _userService.GetSubscriptions(new User { Id = userId });
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("{userId}/liked-photos")]
        public IActionResult GetLikedPhotos(int userId)
        {
            IDataResult<List<Photo>> result = _userService.GetLikedPhotos(new User { Id = userId });
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<PhotoForListDto>>(result.Data);
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }
        [HttpPut]
        public IActionResult Put(UserForUpdateDto userForUpdateDto)
        {
            IResult result = _userService.Update(userForUpdateDto);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpDelete]
        public IActionResult Delete(int userId)
        {
            IResult result = _userService.Delete(new User { Id = userId });
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}