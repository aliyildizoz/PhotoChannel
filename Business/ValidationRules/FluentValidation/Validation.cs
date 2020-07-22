using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Core.Utilities.Business;
using Entities.Abstract;
using Entities.Dtos;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Business.ValidationRules.FluentValidation
{
    public class Validation<TValidator> where TValidator : class, IValidator, new()
    {
        public void Validate(object entity)
        {
            ValidatorFactory factory = new ValidatorFactory();
            IValidator validator = factory.CreateInstance(typeof(TValidator));
            var validateResult = validator.Validate(entity);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }
        }
    }
}
