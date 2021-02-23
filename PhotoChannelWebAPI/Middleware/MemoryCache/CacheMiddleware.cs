using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhotoChannelWebAPI.Cache;
using PhotoChannelWebAPI.Extensions;

namespace PhotoChannelWebAPI.Middleware.MemoryCache
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
            if (httpContext.Request.Method != "GET")
            {
                await _next.Invoke(httpContext);
            }
            else
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

        }

        private void ResponseHandler(HttpContext httpContext, object response)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            httpContext.Response.ContentType = "application/json; charset=utf-8";
            var json = JsonConvert.SerializeObject(response, serializerSettings);
            httpContext.Response.WriteAsync(json);
        }
    }
}
