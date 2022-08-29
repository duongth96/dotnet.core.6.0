﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AutoMapper;
using MediatR;
using EA.NetDevPack.Domain;
using EA.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
 
namespace EA.Core.Application.Events
{
    public  class RoleAddEvent : Event
    {
        public RoleAddEvent(Guid id, string fullName, string role)
        {
            Id = id;
            FullName = fullName;
            Role = role;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
    public class RoleAddEventHandler : INotificationHandler<RoleAddEvent>
    {

        //private readonly IMapper _mapper;
        public RoleAddEventHandler()
        {
          //  _mapper = mapper;
        }

        public Task Handle(RoleAddEvent notification, CancellationToken cancellationToken)
        {
            Thread.Sleep(2000);
            return Task.CompletedTask;
        }
    }
}