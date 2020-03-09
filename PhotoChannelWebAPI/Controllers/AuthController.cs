using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMapper _mapper;
        private IAuthHelper _authHelper;

        public AuthController(IAuthService authService, IMapper mapper, IAuthHelper authHelper)
        {
            _authService = authService;
            _mapper = mapper;
            _authHelper = authHelper;
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
                _authHelper.Login(userToLogin.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("currentuser")]
        public ActionResult GetCurrentUser()
        {
            User user = _authHelper.GetCurrentUser();
            if (user == null)
            {
                return BadRequest();
            }
            CurrentUserDto currentUserDto = _mapper.Map<CurrentUserDto>(user);
            return Ok(currentUserDto);

        }
        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            _authHelper.Logout();
            return Ok();
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
                _authHelper.Login(registerResult.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}