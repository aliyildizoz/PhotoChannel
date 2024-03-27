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
using NLog;
using NLog.Web;

namespace PhotoChannelWebAPI.Helpers
{
    public class JwtEvents : JwtBearerEvents, IJwtEvents
    {
       public override Task TokenValidated(TokenValidatedContext context)
        {
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
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            logger.Warn(context.Result.Failure,"RefreshToken is not required.");
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

    }
    

}
