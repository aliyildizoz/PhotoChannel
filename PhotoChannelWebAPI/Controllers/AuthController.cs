using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.Dtos;

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

        [HttpPost("login")]
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
                var mapResult = _mapper.Map<LoginOrRegisterForReturnDto>(result.Data);
                mapResult.UserId = userToLogin.Data.Id;
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }

        //GetCurrentUser

        [HttpPost("register")]
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
                var mapResult = _mapper.Map<LoginOrRegisterForReturnDto>(result.Data);
                mapResult.UserId = registerResult.Data.Id;
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
    }
}