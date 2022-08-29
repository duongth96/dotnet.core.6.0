using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EA.Core.Domain.Interfaces;
using EA.Core.Domain.Models;
using EA.Core.Infra.Context;
using Microsoft.EntityFrameworkCore;
using EA.NetDevPack.Data;

namespace EA.Core.Infra.Repository
{
    public class PrivilegeRepository : IPrivilegeRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Privileges> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public PrivilegeRepository(SqlCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<Privileges>();
        }

        public PrivilegeRepository()
        {
        }

        public async Task<Privileges> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Privileges>> GetAll()
        {
            return await DbSet.ToListAsync();
        } 

        public void Add(Privileges privilege)
        {
           DbSet.Add(privilege);
        }

        public void Update(Privileges privilege)
        {
            DbSet.Update(privilege);
        }

        public void Remove(Privileges privilege)
        {
            DbSet.Remove(privilege);
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
