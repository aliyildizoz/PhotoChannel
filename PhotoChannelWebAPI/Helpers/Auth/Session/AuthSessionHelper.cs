using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Helpers.Auth.Session
{
    public class AuthSessionHelper : IAuthHelper
    {
        private IHttpContextAccessor _accessor;
        private ISession _session;
        private string currentUserKey = "CurrentUser";
        private string userIdkey = "UserId";

        public AuthSessionHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _session = accessor.HttpContext.Session;
        }

        public void Login(User user)
        {
            HttpContext context = _accessor.HttpContext;
            if (context != null)
            {
                _session.Set(currentUserKey, user);
                _session.SetInt32(userIdkey, user.Id);
            }
        }
        
        public void Logout()
        {
            HttpContext context = _accessor.HttpContext;
            if (context != null)
            {
                _session.Remove(currentUserKey);
                _session.Remove(userIdkey);
            }
        }
        public User GetCurrentUser()
        {
            return _session.Get<User>(currentUserKey);
        }
        public string GetCurrentUserId()
        {
            HttpContext context = _accessor.HttpContext;
            if (context != null)
            {
                var value = _session.GetInt32(userIdkey);
                return value == null ? null : value.ToString();
            }

            return null;
        }
    }
}
