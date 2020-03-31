using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ChannelValidator : AbstractValidator<Channel>
    {
        public ChannelValidator()
        {
            RuleFor(channel => channel.ChannelPhotoUrl).NotNull().MaximumLength(150);
            RuleFor(channel => channel.Name).NotNull().MaximumLength(100);

        }
    }
}
