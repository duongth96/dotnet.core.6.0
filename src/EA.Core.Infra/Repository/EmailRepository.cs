
using EA.Core.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Infra.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger<EmailRepository> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public EmailRepository()
        {
        }

        public EmailRepository(ILogger<EmailRepository> logger, IPublishEndpoint publishEndpoint=null)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Send(string email, string subject, string body)
        {
           // var userEmailEvent = new UserEmailEvent(email,subject,body);
            
           // await _publishEndpoint.Publish(userEmailEvent);

           // var userEmailEvent = new UserLogedEvent(email);

            //await _publishEndpoint.Publish(userEmailEvent);
        }
    }
}
