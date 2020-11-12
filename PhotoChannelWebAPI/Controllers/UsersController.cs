using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
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
        private IAuthService _authService;
        public UsersController(IMapper mapper, IUserService userService, ICountService countService, IAuthService authService)
        {
            _mapper = mapper;
            _userService = userService;
            _countService = countService;
            _authService = authService;
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
            return this.ServerError(result.Message);
        }
        [HttpGet]
        [Route("{userId}")]
        public IActionResult Get(int userId)
        {
            var contains = _userService.Contains(new User { Id = userId });
            if (contains)
            {
                IDataResult<User> result = _userService.GetById(userId);

                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                    mapResult.SubscriptionCount = _countService.GetSubscriptionsCount(userId).Data;
                    return Ok(mapResult);
                }
                return this.ServerError(result.Message);

            }

            return NotFound();
        }

        [HttpPut]
        [Route("{userId}")]
        [Authorize]
        public IActionResult UpdateUserAbout(int userId, UserForUpdateDto userForUpdateDto)
        {
            var userExists = _userService.UserExistsWithUpdate(userForUpdateDto.Email, userId);
            if (userExists.IsSuccessful)
            {
                return BadRequest(userExists.Message);
            }
            _authService.TokenExpiration(userId);
            var oldUserResult = _userService.GetById(userId);
            if (oldUserResult.IsSuccessful)
            {
                oldUserResult.Data.Email = userForUpdateDto.Email;
                oldUserResult.Data.FirstName = userForUpdateDto.FirstName;
                oldUserResult.Data.LastName = userForUpdateDto.LastName;
                oldUserResult.Data.UserName = userForUpdateDto.UserName;
                IResult result = _userService.Update(oldUserResult.Data);
                if (result.IsSuccessful)
                {
                    var accessToken = _authService.CreateAccessToken(oldUserResult.Data);
                    return Ok(accessToken.Data);
                }
            }

            return this.ServerError(oldUserResult.Message);
        }
        [HttpPut]
        [Route("{userId}/password")]
        [Authorize]
        public IActionResult UpdatePassword(int userId, PasswordUpdateDto passwordUpdateDto)
        {
            //todo: gelen idnin mevcut kullnaıcı olup olmadığının kontrolü
            var oldUserResult = _userService.GetById(userId);
            if (oldUserResult.IsSuccessful)
            {
                var verifyPassword = new UserForPasswordDto
                {
                    Password = passwordUpdateDto.OldPassword,
                    PasswordHash = oldUserResult.Data.PasswordHash,
                    PasswordSalt = oldUserResult.Data.PasswordSalt
                };
                if (!HashingHelper.VerifyPasswordHash(verifyPassword))
                {
                    return BadRequest(Messages.UpdatePasswordError);
                }

                IResult result = _userService.UpdatePassword(oldUserResult.Data, passwordUpdateDto.NewPassword);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }
            return NotFound(oldUserResult.Message);
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
            return this.ServerError(result.Message);
        }
    }
}