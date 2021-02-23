using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhotoChannelWebAPI.Cache;
using PhotoChannelWebAPI.CustomActionResults;
using PhotoChannelWebAPI.Exceptions;

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
        public static void CacheFill(this ControllerBase controller, object value)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            var key = controller.Request.Path.Value;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            memoryCache.Set(key, value, options);
        }
        public static void CacheFillWithUserId(this ControllerBase controller, object value)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            var key = controller.Request.Path.Value;
            var userId = controller.User.Claims.GetUserId();
            if (!userId.IsSuccessful)
            {
                throw new NoUserIdException();
            }

            key = userId.Data + key;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            memoryCache.Set(key, value, options);
        }
        public static void CacheFill(this ControllerBase controller, object value, MemoryCacheEntryOptions options)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            var key = controller.Request.Path.Value;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
            memoryCache.Set(key, value, options);
        }
        public static void RemoveCache(this ControllerBase controller)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            var key = controller.Request.Path.Value.Split('/')[2];
            var result = controller.User.Claims.GetUserId();

            memoryCache.RemoveRange(memoryCache.Keys.Where(o => ((string)o).ToLower().Contains(key.ToLower())).ToList());
        }
        public static void CacheClear(this ControllerBase controller)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            memoryCache.Clear();
        }
        public static void RemoveCacheByContains(this ControllerBase controller, string key)
        {
            var memoryCache = controller.HttpContext.RequestServices.GetService<ICustomMemoryCache>();
            var result = memoryCache.Keys.FirstOrDefault(o => ((string)o).ToLower().Contains(key.ToLower()));
            if (result != null)
            {
                memoryCache.Remove(result);
            }
        }
    }
}
