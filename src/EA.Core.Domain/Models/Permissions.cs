﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using EA.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace EA.Core.Domain.Models
{
    public partial class Permissions : Entity, IAggregateRoot
    {
        public Permissions()
        {
            Privileges = new HashSet<Privileges>();
        }

        public string Code { get; set; }
        public string Name { get; set; } 

        public virtual ICollection<Privileges> Privileges { get; set; }
    }
}