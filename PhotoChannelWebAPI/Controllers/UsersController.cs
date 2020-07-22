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
using Microsoft.AspNetCore.Authorization;
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
        private ICountService _countService;
        public UsersController(IMapper mapper, IUserService userService, ICountService countService)
        {
            _mapper = mapper;
            _userService = userService;
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
        [Route("{userId}/updateuserabout")]
        [Authorize]
        public IActionResult UpdateUserAbout(int userId, UserForUpdateDto userForUpdateDto)
        {
            var userExists = _userService.UserExistsWithUpdate(userForUpdateDto.Email, userId);
            if (userExists.IsSuccessful)
            {
                return BadRequest(userExists.Message);
            }

            var oldUserResult = _userService.GetById(userId);
            if (oldUserResult.IsSuccessful)
            {
                oldUserResult.Data.Email = userForUpdateDto.Email;
                oldUserResult.Data.FirstName = userForUpdateDto.FirstName;
                oldUserResult.Data.LastName = userForUpdateDto.LastName;
                IResult result = _userService.UpdateUserAbout(oldUserResult.Data);
                if (result.IsSuccessful)
                {
                    //User user = _userService.GetById(userId).Data;
                    //Todo:Güncellendiğinde yeni bir accsesstoken yarat
                    return Ok(result.Message);
                }
            }

            return BadRequest(oldUserResult.Message);
        }
        [HttpPut]
        [Route("{userId}/updatepassword")]
        [Authorize]
        public IActionResult UpdatePassword(int userId, string password)
        {
            var oldUserResult = _userService.GetById(userId);
            if (oldUserResult.IsSuccessful)
            {
                IResult result = _userService.UpdatePassword(oldUserResult.Data, password);
                if (result.IsSuccessful)
                {
                    //User user = _userService.GetById(userId).Data;
                    return Ok(result.Message);
                }
            }
            return BadRequest(oldUserResult.Message);
        }
        [HttpDelete]
        [Route("{userId}")]
        [Authorize]
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