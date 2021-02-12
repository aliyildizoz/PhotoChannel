using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PhotoChannelWebAPI.Cache;
using PhotoChannelWebAPI.Exceptions;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Middlewares.MemoryCache
{
    public class CacheMiddleware
    {
        private readonly RequestDelegate _next;
        private string[] _addUserIdKeys = { "issub", "islike" };
        public CacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            ICustomMemoryCache memoryCache = httpContext.RequestServices.GetService<ICustomMemoryCache>();
            var key = httpContext.Request.Path.Value;
            var pathSplit = key.Split("/");
            if (pathSplit.Length >= 4 && _addUserIdKeys.Contains(pathSplit[3].ToLower()))
            {
                var userId = httpContext.User.Claims.GetUserId().Data;
                key = userId + key;
            }

            if (memoryCache.TryGetValue(key, out var value))
            {
                ResponseHandler(httpContext, value);
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }

        private void ResponseHandler(HttpContext httpContext, object response)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
