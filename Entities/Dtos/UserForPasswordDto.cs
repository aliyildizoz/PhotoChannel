using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Dtos
{
    public class UserForPasswordDto : IDto
    {
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
