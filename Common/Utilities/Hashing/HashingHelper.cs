using System;
using System.Collections.Generic;
using System.Text;
using Entities.Dtos;

namespace Core.Utilities.Hashing
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(UserForPasswordDto userForPasswordDto)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                userForPasswordDto.PasswordSalt = hmac.Key;
                userForPasswordDto.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userForPasswordDto.Password));
            }
        }

        public static bool VerifyPasswordHash(UserForPasswordDto userForPasswordDto)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userForPasswordDto.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userForPasswordDto.Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userForPasswordDto.PasswordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
