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
    public  class UserAddEvent : Event
    {

        public UserAddEvent(Guid id, string loginId, string fullName)
        {
            Id = id;
            LoginID = loginId;
            FullName = fullName;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string LoginID { get; set; }
        public string FullName { get; set; }
         
    }
    public class UserAddEventHandler : INotificationHandler<UserAddEvent>
    {

        //private readonly IMapper _mapper;
        public UserAddEventHandler()
        {
          //  _mapper = mapper;
        }

        public Task Handle(UserAddEvent notification, CancellationToken cancellationToken)
        {
            Thread.Sleep(2000);
            return Task.CompletedTask;
        }

    }
}