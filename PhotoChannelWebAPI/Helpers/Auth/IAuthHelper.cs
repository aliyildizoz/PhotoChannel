using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace PhotoChannelWebAPI.Helpers
{
    public interface IAuthHelper
    {
        void Login(User user);
        void Logout();
        User GetCurrentUser();
        string GetCurrentUserId();
    }
}
