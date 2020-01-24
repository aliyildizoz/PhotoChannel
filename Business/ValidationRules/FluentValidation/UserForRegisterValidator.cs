using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForRegisterValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterValidator()
        {
            RuleFor(user => user.FirstName).NotNull().MaximumLength(50);
            RuleFor(user => user.LastName).NotNull().MaximumLength(50);
            RuleFor(user => user.Email).NotNull().MaximumLength(50);
            RuleFor(user => user.UserName).NotNull().MaximumLength(50);
            RuleFor(user => user.Password).NotNull();
        }
    }
}
