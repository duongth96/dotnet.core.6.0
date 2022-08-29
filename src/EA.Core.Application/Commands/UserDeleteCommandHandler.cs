using FluentValidation.Results;
using EA.Core.Domain.Interfaces;
using EA.Core.Domain.Models;
using MediatR;
using EA.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA.Core.Application.Events;

namespace EA.Core.Application.Commands
{
    public class UserDeleteCommandHandler : CommandHandler, IRequestHandler<UserDeleteCommand, ValidationResult>
    {
        private readonly IUserRepository _userRepository;

        public UserDeleteCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ValidationResult> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var user = new Users
            {
                Id = request.Id,
                LoginID = request.LoginID,
                FullName = request.FullName,
            };

            //add domain event
            user.AddDomainEvent(new UserAddEvent(user.Id, user.LoginID, user.FullName));

            _userRepository.Remove(user);
            return await Commit(_userRepository.UnitOfWork);
        }
        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
