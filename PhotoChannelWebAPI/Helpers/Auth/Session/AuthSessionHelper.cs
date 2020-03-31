using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
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
        private IUserService _userService;

        public AuthSessionHelper(IHttpContextAccessor accessor, IUserService userService)
        {
            _accessor = accessor;
            _userService = userService;
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
            string id = GetCurrentUserId();
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return _userService.GetById(int.Parse(id)).Data;
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
