using System;
using AutoMapper;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Helpers;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
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
        [HttpGet]
        [Route("refreshtoken")]
        public ActionResult RefreshToken([FromHeader]string refreshToken)
        {
            var result = _authService.CreateRefreshToken(refreshToken);
            if (result.IsSuccessful) return Ok(result.Data);

            return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("currentuser")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            var result = User.Claims.GetCurrentUser();
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var id = User.Claims.GetUserId();
            if (id.IsSuccessful)
            {
                var result = _authService.TokenExpiration(id.Data);
                if (result.IsSuccessful)
                {
                    return Ok();
                }
            }
           
            return BadRequest();
        }
        [HttpPost]
        [Route("register")]
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
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}