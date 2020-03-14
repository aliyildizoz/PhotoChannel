using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            IDataResult<User> result = _userService.GetByEmail(userForLoginDto.Email);
            if (result.Data != null)
            {
                UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                {
                    Password = userForLoginDto.Password,
                    PasswordHash = result.Data.PasswordHash,
                    PasswordSalt = result.Data.PasswordSalt
                };

                if (!HashingHelper.VerifyPasswordHash(userForPasswordDto))
                {
                    return new ErrorDataResult<User>(Messages.PasswordError, result.Data);
                }
                return new SuccessDataResult<User>(Messages.SuccessfulLogin, result.Data);
            }
            return new ErrorDataResult<User>(Messages.PasswordAndUsernameError, result.Data);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {

            if (!UserExists(userForRegisterDto.Email).IsSuccessful)
            {
                UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                {
                    Password = userForRegisterDto.Password
                };
                HashingHelper.CreatePasswordHash(userForPasswordDto);
                User user = new User
                {
                    Email = userForRegisterDto.Email,
                    FirstName = userForRegisterDto.FirstName,
                    LastName = userForRegisterDto.LastName,
                    UserName = userForRegisterDto.UserName,
                    PasswordHash = userForPasswordDto.PasswordHash,
                    PasswordSalt = userForPasswordDto.PasswordSalt
                };
                IDataResult<User> result = _userService.Add(user);
                if (!result.IsSuccessful)
                {
                    return new ErrorDataResult<User>(result.Message, user);
                }
                return new SuccessDataResult<User>(result.Message, result.Data);
            }
            return new ErrorDataResult<User>(Messages.UserAlreadyExists, null);
        }

        public IResult UserExists(string email)
        {
            return _userService.UserExists(email);
        }
        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var dataResult = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, dataResult.Data);
            return new SuccessDataResult<AccessToken>(Messages.AccessTokenCreated, accessToken);
        }
    }
}
