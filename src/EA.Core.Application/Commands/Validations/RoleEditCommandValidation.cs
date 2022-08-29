using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Application.Commands.Validations
{
    public class RoleEditCommandValidation : RoleValidation<RoleEditCommand>
    {
        public RoleEditCommandValidation()
        {
            ValidateFullName();
            ValidateRole();
        }
    }
}