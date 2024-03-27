using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using NLog.Web;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                logger.Info("Request by user id:{0}", context.HttpContext.User.Claims.GetUserId().Data);
            }
            logger.Info("General request");
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            var statusCode = context.HttpContext.Response.StatusCode;
            if (statusCode == 500)
            {
                logger.Error("Response status code : {0} ", statusCode);
                return;
            }

            logger.Info("Response status code : {0} ", statusCode);
        }
    }
}
