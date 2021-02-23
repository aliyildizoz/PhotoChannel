using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PhotoChannelWebAPI.CustomActionResults;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Exceptions;

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
                ExceptionHandler(exception, httpContext, JsonConvert.SerializeObject(exceptionResponse), HttpStatusCode.BadRequest, "application/json");
            }
            catch (NoUserIdException exception)
            {
                ExceptionHandler(exception, httpContext, exception.Message, HttpStatusCode.NotFound);
            }
            catch (System.Exception exception)
            {
                ExceptionHandler(exception, httpContext, JsonConvert.SerializeObject(exception), contentType: "application/json");
            }
        }
        private void ExceptionHandler(System.Exception exception, HttpContext httpContext, string content, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string contentType = "text/plain")
        {
            //todo: log
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = contentType;
            httpContext.Response.WriteAsync(content);

        }
    }
}
