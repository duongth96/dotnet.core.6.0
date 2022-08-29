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
    public class UserCommandHandler : CommandHandler, IRequestHandler<UserAddCommand, ValidationResult>
    {
        private readonly IUserRepository _userRepository;

        public UserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ValidationResult> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var user = new Users
            {
                Id = request.Id,
                Sub = request.Id.ToString(),
                LoginID = request.LoginID,
                LoginName = request.LoginName,
                FullName = request.FullName,
                Email = request.Email,
                Address = request.Address,
                Phone = request.Phone,
                Photo = request.Photo,
                Status = request.Status,
                CreatedDate = DateTime.Now,
                CreatedBy = request.CreatedBy
            };

            //add domain event
            user.AddDomainEvent(new UserAddEvent(user.Id, user.LoginID, user.FullName));

            _userRepository.Add(user);
            return await Commit(_userRepository.UnitOfWork);
        }
        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
