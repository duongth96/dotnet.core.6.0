using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task Send(string email, string subject, string body);
    }
}
