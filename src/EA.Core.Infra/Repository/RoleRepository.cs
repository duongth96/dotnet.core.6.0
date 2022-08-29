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
    public class RoleRepository : IRoleRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Roles> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public RoleRepository(SqlCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<Roles>();
        }

        public RoleRepository()
        {
        }

        public async Task<Roles> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Roles>> GetAll()
        {
            return await DbSet.ToListAsync();
        } 

        public void Add(Roles role)
        {
           DbSet.Add(role);
        }

        public void Update(Roles role)
        {
            DbSet.Update(role);
        }

        public void Remove(Roles role)
        {
            DbSet.Remove(role);
        }

        public void Dispose()
        {
          Db.Dispose();
        }

        public async Task<IEnumerable<Roles>> Query(string keyword, int pagesize, int pageindex)
        {
            if(!string.IsNullOrEmpty(keyword))
                return await DbSet.Where(x => x.FullName.Contains(keyword) || x.Role.Contains(keyword)).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
            else
                return await DbSet.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> Count(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await DbSet.Where(x => x.FullName.Contains(keyword) || x.Role.Contains(keyword)).CountAsync();
            else
                return await DbSet.CountAsync();
        }
    }
}
