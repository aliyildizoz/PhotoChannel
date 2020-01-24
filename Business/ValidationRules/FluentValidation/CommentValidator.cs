using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Description).NotNull().MaximumLength(500);
            RuleFor(comment => comment.UserId).NotNull();
            RuleFor(comment => comment.PhotoId).NotNull();
        }
    }
}
