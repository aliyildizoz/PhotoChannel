using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Security.Jwt;

namespace PhotoChannelWebAPI.Dtos
{
    public class LoginOrRegisterForReturnDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int UserId { get; set; }
    }
}
