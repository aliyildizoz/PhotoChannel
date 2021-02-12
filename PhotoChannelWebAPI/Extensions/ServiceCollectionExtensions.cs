using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhotoChannelWebAPI.Cache;

namespace PhotoChannelWebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services,
            Action<MemoryCacheOptions> setupAction)
        {
            services.AddMemoryCache(setupAction);
            services.AddSingleton<ICustomMemoryCache, CustomMemoryCache>();
            return services;
        }
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICustomMemoryCache, CustomMemoryCache>();
            return services;
        }
    }
}
