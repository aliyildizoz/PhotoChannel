using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PhotoValidator : AbstractValidator<Photo>
    {
        public PhotoValidator()
        {
            RuleFor(photo => photo.ChannelId).NotNull();
            RuleFor(photo => photo.UserId).NotNull();
            RuleFor(photo => photo.PhotoUrl).NotNull().MaximumLength(150);
        }
    }
}
