using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using PhotoChannelWebAPI.Middlewares.Exception;

namespace PhotoChannelWebAPI.Middlewares.MemoryCache
{
    public static class MemoryCacheMiddlewareExtension
    {
        public static IApplicationBuilder UseCacheMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheMiddleware>();
        }
    }
}
