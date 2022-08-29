using AutoMapper;
using EA.Core.Domain.Interfaces;
using MediatR;
using EA.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Application.Events
{
    public class UserEmailEvent : Event
    {
        public UserEmailEvent()
        {
        }

        public UserEmailEvent(string email, string subject, string body)
        {
            Email = email;
            Subject = subject;
            Body = body;
        } 
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class UserEmailEventHandler : INotificationHandler<UserEmailEvent>
    {
        private readonly IEmailRepository _emailRepository;
        //private readonly IMapper _mapper;
        
        public UserEmailEventHandler(IEmailRepository emailRepository, IMapper mapper)
        {
            _emailRepository = emailRepository;
           // _mapper = mapper;
        }

        public UserEmailEventHandler()
        {
        }

        public Task Handle(UserEmailEvent notification, CancellationToken cancellationToken)
        {
            _emailRepository.Send(notification.Email, notification.Subject, notification.Body);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
             
        }

    }
}
