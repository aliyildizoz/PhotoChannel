using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Extensions;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace PhotoChannelWebAPI.Helpers.Auth.Cookie
{
    public class AuthCookieHelper : IAuthHelper
    {
        #region CookieConfiguration
        //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        //{
        //    options.Cookie.Name = "auth_cookie";
        //    options.Cookie.SameSite = SameSiteMode.None;
        //});
        #endregion
        private IHttpContextAccessor _accessor;
        private IUserService _userService;
        public AuthCookieHelper(IHttpContextAccessor accessor, IUserService userService)
        {
            _accessor = accessor;
            _userService = userService;
        }

        public void Login(User user)
        {
            HttpContext? context = _accessor.HttpContext;
            if (context != null)
            {
                var dataResult = _userService.GetClaims(user.Id);
                if (dataResult.IsSuccessful)
                {
                    var claims = new List<Claim>();
                    claims.AddFirstName($"{user.FirstName} {user.LastName}");
                    claims.AddNameIdentifier(user.Id.ToString());
                    claims.AddUserName(user.UserName);
                    claims.AddRoles(dataResult.Data.Select(claim => claim.ClaimName).ToArray());
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };
                    context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity), authProperties);
                }
            }

        }
       
        public void Logout()
        {
            HttpContext? context = _accessor.HttpContext;
            if (context != null)
            {
                context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public User GetCurrentUser()
        {
            HttpContext? context = _accessor.HttpContext;
            if (context != null)
            {
                int userId = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (userId > 0)
                {
                    var dataResult = _userService.GetById(userId);
                    if (dataResult.IsSuccessful)
                    {
                        return dataResult.Data;
                    }
                }
            }

            return null;
        }

        public string? GetCurrentUserId()
        {
            string? id = "";
            HttpContext? context = _accessor.HttpContext;
            if (context != null)
            {
                id = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return id;
        }
    }
}
