using System;
using AutoMapper;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Filters;
using PhotoChannelWebAPI.Helpers;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [LogFilter]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Logins a user
        /// </summary>
        /// <param name="userForLoginDto"></param>
        /// <response code="200">Returns the newly created access token.</response>
        /// <response code="400">If the user is null or the user is not found.</response>    
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.IsSuccessful)
            {
                return BadRequest(userToLogin.Message);
            }
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Creates a new access token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <response code="200">Returns the newly created access token</response>  
        /// <response code="400">If the refreshToken is not valid</response>    
        [HttpGet]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RefreshToken([FromHeader] string refreshToken)
        {
            var result = _authService.CreateRefreshToken(refreshToken);
            if (result.IsSuccessful) return Ok(result.Data);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Gets logged current user
        /// </summary>
        /// <response code="400">If the current user is null</response>    
        /// <response code="401">If the user is unauthorize</response>     
        [HttpGet]
        [Route("currentuser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult GetCurrentUser()
        {
            var result = User.Claims.GetCurrentUser();
            if (result.IsSuccessful)
            {
                this.CacheFill(result.Data);
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
       
        /// <summary>
        /// Logouts current user
        /// </summary>
        /// <response code="200">If the current user is logout</response>
        /// <response code="400">If the current user is null</response>    
        /// <response code="401">If the user is unauthorize</response>  
        [HttpGet]
        [Route("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Logout()
        {
            var id = User.Claims.GetUserId();
            if (id.IsSuccessful)
            {
                var result = _authService.TokenExpiration(id.Data);
                if (result.IsSuccessful)
                {
                    this.CacheClear();
                    return Ok();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userForRegisterDto"></param>
        /// <returns>Returns a newly created access token</returns>
        /// <response code="200">Returns the newly created access token</response>
        /// <response code="400">If the user is null</response>    
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (userExists.IsSuccessful)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _authService.Register(userForRegisterDto);
            if (!registerResult.IsSuccessful)
            {
                return BadRequest(registerResult.Message);
            }
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.IsSuccessful)
            {
                this.RemoveCache();
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}