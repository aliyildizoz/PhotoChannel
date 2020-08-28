using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.AutoMapperConfig;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IMapper _mapper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = BusinessMapperCfg.Instance.Mapper();
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

            Validation<UserForRegisterValidator> validation = new Validation<UserForRegisterValidator>();
            validation.Validate(userForRegisterDto);

            var user = _mapper.Map<User>(userForRegisterDto);
            if (!UserExists(userForRegisterDto.Email).IsSuccessful)
            {
                UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                {
                    Password = userForRegisterDto.Password
                };
                HashingHelper.CreatePasswordHash(userForPasswordDto);


                user.PasswordHash = userForPasswordDto.PasswordHash;
                user.PasswordSalt = userForPasswordDto.PasswordSalt;
                user.IsActive = false;

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

        public IDataResult<AccessToken> CreateRefreshToken(string refreshToken)
        {

            var byRefreshToken = _userService.GetByRefreshToken(refreshToken);
            if (byRefreshToken.IsSuccessful)
            {
                return CreateAccessToken(byRefreshToken.Data);
            }

            return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
        }

        public IResult TokenExpiration(int userId)
        {
            var dataResult = _userService.GetClaims(userId);
            var userDataResult = _userService.GetById(userId);
            userDataResult.Data.RefreshToken = _tokenHelper.CreateRefreshToken();
            var result = _userService.UpdateRefreshToken(userDataResult.Data);
            if (result.IsSuccessful)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult RefreshTokenValidate(string refreshToken)
        {
            var result = _userService.GetByRefreshToken(refreshToken);
            if (result.IsSuccessful)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.UserNotFound);
        }


        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var dataResult = _userService.GetClaims(user.Id);
            var accessToken = _tokenHelper.CreateToken(user, dataResult.Data);
            if (accessToken == null)
            {
                return new ErrorDataResult<AccessToken>();
            }

            user.RefreshToken = accessToken.RefreshToken;
            _userService.UpdateRefreshToken(user);
            return new SuccessDataResult<AccessToken>(Messages.AccessTokenCreated, accessToken);
        }
    }
}
