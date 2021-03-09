using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhotoChannelWebAPI.Exceptions;

namespace PhotoChannelWebAPI.Filters
{
    public class ContainsFilterAttribute : ActionFilterAttribute
    {
        private Type _service;
        private Type _entity;
        private string _propertyName;
        public ContainsFilterAttribute(Type service, Type entity)
        {
            if (!typeof(ICommonService).IsAssignableFrom(service) || !typeof(IEntity).IsAssignableFrom(entity))
            {
                throw new System.Exception("Service or entity undefined.");
            }
            _service = service;
            _entity = entity;
        }
        public ContainsFilterAttribute(Type service, Type entity, string propertyName)
        {
            if (!typeof(ICommonService).IsAssignableFrom(service) || !typeof(IEntity).IsAssignableFrom(entity))
            {
                throw new System.Exception("Service or entity undefined.");
            }
            _service = service;
            _entity = entity;
            _propertyName = propertyName;
        }
      
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (ICommonService)context.HttpContext.RequestServices.GetService(_service);
            var entity = (IEntity)Activator.CreateInstance(_entity);

            if (_propertyName == null)
            {
                entity.Id = (int)context.ActionArguments.FirstOrDefault(pair => pair.Key.ToUpper().Contains("ID"))
                    .Value;
            }
            else
            {
                var value = context.ActionArguments.FirstOrDefault(pair => pair.Value.GetType().IsClass).Value;
                entity.Id = Convert.ToInt16(value.GetType().GetProperties().First(info => info.Name == _propertyName)
                    .GetValue(obj: value));
            }

            if (!service.Contains(entity))
            {
                throw new EntityNotFoundException(entity.GetType().Name);
            }
        }


    }

}
