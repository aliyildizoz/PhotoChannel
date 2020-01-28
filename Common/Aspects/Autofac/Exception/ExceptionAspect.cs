using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Core.Utilities.Interceptor;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using FluentValidation;

namespace Core.Aspects.Autofac.Exception
{
    public class ExceptionAspect : MethodInterception
    {
        private Type _returnType;
        public ExceptionAspect(Type returnType)
        {
            if (!typeof(IResult).IsAssignableFrom(returnType))
            {
                throw new System.Exception(AspectMessages.WrongReturnType);
            }

            _returnType = returnType;
        }
        protected override void OnException(IInvocation invocation, System.Exception exception)
        {
            if (!_returnType.IsGenericType)
            {
                invocation.ReturnValue = new ErrorResult(exception.Message);
                return;
            }

            //invocation.ReturnValue = new ErrorDataResult<>(exception.Message);
            return;

        }
    }
}
