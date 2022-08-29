using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Application.Commands.Validations
{
    public abstract class RoleValidation<T> : AbstractValidator<T> where T : RoleCommand 
    {
        protected void ValidateRole()
        {
            RuleFor(c => c.Role)
                .NotEmpty().WithMessage("Please ensure you have entered the Role")
                .Length(2, 25).WithMessage("The Role must have between 2 and 25 characters");
        }
        protected void ValidateFullName()
        {
            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

    }
}
