using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Application.Commands.Validations
{
    public abstract class UserValidation<T> : AbstractValidator<T> where T : UserCommand 
    {
        protected void ValidateUserName()
        {
            RuleFor(c => c.LoginID)
                .NotEmpty().WithMessage("Please ensure you have entered the UserName")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

    }
}
