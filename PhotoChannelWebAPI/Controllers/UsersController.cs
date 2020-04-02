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
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private IAuthHelper _authHelper;
        private ICountService _countService;
        public UsersController(IMapper mapper, IUserService userService, IAuthHelper authHelper, ICountService countService)
        {
            _mapper = mapper;
            _userService = userService;
            _authHelper = authHelper;
            _countService = countService;
        }
        [HttpGet]
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
        [HttpGet]
        [Route("{userId}")]
        public IActionResult Get(int userId)
        {
            IDataResult<User> result = _userService.GetById(userId);

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                mapResult.SubscriptionCount = _countService.GetSubscriptionsCount(userId).Data;
                return Ok(mapResult);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        [Route("{userId}")]
        public IActionResult Put(int userId, UserForUpdateDto userForUpdateDto)
        {
            var userExists = _userService.UserExistsWithUpdate(userForUpdateDto.Email, userId);
            if (userExists.IsSuccessful)
            {
                return BadRequest(userExists.Message);
            }
            IResult result = _userService.Update(userForUpdateDto, userId);
            if (result.IsSuccessful)
            {
                User user = _userService.GetById(userId).Data;
                _authHelper.Login(user);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpDelete]
        [Route("{userId}")]
        public IActionResult Delete(int userId)
        {
            IResult result = _userService.Delete(userId);
            if (result.IsSuccessful)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}