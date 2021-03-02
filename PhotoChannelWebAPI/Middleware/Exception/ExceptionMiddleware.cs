using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using PhotoChannelWebAPI.CustomActionResults;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Exceptions;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace PhotoChannelWebAPI.Middleware.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (ValidationException exception)
            {
                ValidationExceptionResponse exceptionResponse = new ValidationExceptionResponse();
                exceptionResponse.Errors = exceptionResponse.Errors;
                exceptionResponse.Message = exceptionResponse.Message;
                ExceptionHandler(exception, httpContext, JsonConvert.SerializeObject(exceptionResponse), LogLevel.Warn,
                    HttpStatusCode.BadRequest, "application/json");
            }
            catch (EntityNotFoundException exception)
            {
                ExceptionHandler(exception, httpContext, exception.Message, LogLevel.Warn, HttpStatusCode.NotFound);
            }
            catch (NoUserIdException exception)
            {
                ExceptionHandler(exception, httpContext, exception.Message, LogLevel.Error, HttpStatusCode.NotFound);
            }
            catch (System.Exception exception)
            {
                ExceptionHandler(exception, httpContext, StatusCodeMessages.InernalServerError, LogLevel.Fatal);
            }
        }
        private void ExceptionHandler(System.Exception exception, HttpContext httpContext, string content, LogLevel level, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string contentType = "text/plain")
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Log(level, exception, exception.Message);
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = contentType;
            httpContext.Response.WriteAsync(content);

        }
    }
}
