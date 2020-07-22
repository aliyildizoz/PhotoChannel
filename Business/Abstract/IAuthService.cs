using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto);

        IResult UserExists(string email);

        IDataResult<AccessToken> CreateRefreshToken(string refreshToken);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IResult TokenExpiration(int userId);
        IResult RefreshTokenValidate(string refreshToken);
    }
}
