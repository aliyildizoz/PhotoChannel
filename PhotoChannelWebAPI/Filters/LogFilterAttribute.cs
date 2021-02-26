using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PhotoChannelWebAPI.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("Request");
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
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
