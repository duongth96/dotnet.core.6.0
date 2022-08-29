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
    public class RoleAddCommandHandler : CommandHandler, IRequestHandler<RoleAddCommand, ValidationResult>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleAddCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<ValidationResult> Handle(RoleAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var role = new Roles
            {
                Id = request.Id,
                FullName = request.FullName,
                Role = request.Role,
                Description = request.Description,
                Status = request.Status,
                CreatedDate = DateTime.Now,
                CreatedBy = request.CreatedBy
            };

            //add domain event
            role.AddDomainEvent(new RoleAddEvent(role.Id, role.FullName, role.Role));

            _roleRepository.Add(role);
            return await Commit(_roleRepository.UnitOfWork);
        }
        public void Dispose()
        {
            _roleRepository.Dispose();
        }
    }
}
