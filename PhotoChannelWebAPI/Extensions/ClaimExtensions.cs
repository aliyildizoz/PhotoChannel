using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Constants;
using Core.Extensions;
using Core.Utilities.Results;
using Entities.Concrete;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Extensions
{
    public static class ClaimExtensions
    {
        public static IDataResult<CurrentUserDto> GetCurrentUser(this IEnumerable<Claim> claims)
        {
            CurrentUserDto currentUserDto = new CurrentUserDto();
            bool isValid = false;
            foreach (var userClaim in claims)
            {
                switch (userClaim.Type)
                {
                    case CustomClaimTypes.FirstName:
                        currentUserDto.FirstName = userClaim.Value;
                        break;
                    case CustomClaimTypes.LastName:
                        currentUserDto.LastName = userClaim.Value;
                        break;
                    case ClaimTypes.Email:
                        currentUserDto.Email = userClaim.Value;
                        break;
                    case CustomClaimTypes.UserName:
                        currentUserDto.UserName = userClaim.Value;
                        break;
                    case ClaimTypes.NameIdentifier:
                        currentUserDto.Id = Convert.ToInt32(userClaim.Value);
                        isValid = true;
                        break;
                }
            }

            if (isValid)
            {
                return new SuccessDataResult<CurrentUserDto>(currentUserDto);
            }
            return new ErrorDataResult<CurrentUserDto>(Messages.UserNotFound);
        }
        public static IDataResult<int> GetUserId(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return new DataResultBase<int>(false, 0);
            }
            return new DataResultBase<int>(true, Convert.ToInt32(claim.Value));
        }
    }
}
