using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace PhotoChannelWebAPI.Helpers
{
    public class JwtEvents : JwtBearerEvents, IJwtEvents
    {
       public override Task TokenValidated(TokenValidatedContext context)
        {
            Debug.WriteLine("TokenValidated çalıştı");
            if (IsPathIgnore(context.HttpContext.Request.Path.Value)) return Task.CompletedTask;

            string refreshToken = context.Request.Headers["refreshToken"].ToString();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                var result = authService.RefreshTokenValidate(refreshToken);
                if (result.IsSuccessful)
                {
                    return Task.CompletedTask;
                }
            }
            context.Fail("RefreshToken is not required.");

            return Task.CompletedTask;
        }
        private bool IsPathIgnore(string path)
        {
            string[] ignorePaths = { "/api/auth/logout" };
            bool isIgnore = false;
            foreach (var ignorePath in ignorePaths)
            {
                if (ignorePath == path)
                {
                    isIgnore = true;
                }
            }

            return isIgnore;
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            Debug.WriteLine("AuthenticationFailed çalıştı");
            return base.AuthenticationFailed(context);
        }

        public override Task Forbidden(ForbiddenContext context)
        {
            Debug.WriteLine("Forbidden çalıştı");
            return base.Forbidden(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {

            Debug.WriteLine("MessageReceived çalıştı");
            return base.MessageReceived(context);
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            Debug.WriteLine("Challenge çalıştı");
            return base.Challenge(context);
        }
    }
    

}
