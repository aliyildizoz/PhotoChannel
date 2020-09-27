using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoChannelWebAPI.CustomActionResults;

namespace PhotoChannelWebAPI.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ServerErrorObjectResult ServerError(this ControllerBase controller, object value)
        {
            return new ServerErrorObjectResult(value) { StatusCode = StatusCodes.Status500InternalServerError };
        }
        public static ServerErrorObjectResult ServerError(this ControllerBase controller)
        {
            return new ServerErrorObjectResult(StatusCodeMessages.InernalServerError) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
