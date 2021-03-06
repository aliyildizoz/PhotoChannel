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
using PhotoChannelWebAPI.Filters;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [LogFilter]
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

        /// <summary>
        /// Gets all user
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult Get()
        {
            IDataResult<List<User>> result = _userService.GetList();

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<UserForListDto>>(result.Data);
                if (mapResult.Count > 0)
                {
                    this.CacheFill(mapResult);
                }
                return Ok(mapResult);
            }
            return this.ServerError(result.Message);
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <response code="404">If the user is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IUserService), typeof(User))]
        [HttpGet]
        [Route("{userId}")]
        public IActionResult Get(int userId)
        {
            IDataResult<User> result = _userService.GetById(userId);

            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                mapResult.SubscriptionCount = _countService.GetSubscriptionsCount(userId).Data;
                this.CacheFill(mapResult);
                return Ok(mapResult);
            }
            return this.ServerError(result.Message);
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="userForUpdateDto"></param>
        /// <param name="userId">User's id</param>
        /// <response code="200">A newly created access token.</response>
        /// <response code="400">If the username already exists.</response>
        /// <response code="400">If the channel couldn't be updated.</response>
        /// <response code="404">If the user not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ContainsFilter(typeof(IUserService), typeof(User))]
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
                    this.RemoveCacheByContains("users/" + userId);
                    return Ok(accessToken.Data);
                }
            }

            return this.ServerError(oldUserResult.Message);
        }

        /// <summary>
        /// Resets password
        /// </summary>
        /// <param name="passwordUpdateDto"></param>
        /// <param name="userId">User's id</param>
        /// <response code="400">If the user id and current user id are not equal</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the user is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("{userId}/password")]
        [Authorize]
        public IActionResult UpdatePassword(int userId, PasswordUpdateDto passwordUpdateDto)
        {
            if (userId != User.Claims.GetUserId().Data)
            {
                return BadRequest();
            }
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
                    this.RemoveCacheByContains("users/" + userId);
                    return Ok(result.Message);
                }
                return this.ServerError(result.Message);
            }
            return NotFound(oldUserResult.Message);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <response code="400">If the user id and current user id are not equal</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Route("{userId}")]
        [Authorize]
        public IActionResult Delete(int userId)
        {
            if (userId != User.Claims.GetUserId().Data)
            {
                return BadRequest();
            }
            IResult result = _userService.Delete(userId);
            if (result.IsSuccessful)
            {
                this.RemoveCacheByContains("users/" + userId);
                return Ok(result.Message);
            }
            return this.ServerError(result.Message);
        }
    }
}