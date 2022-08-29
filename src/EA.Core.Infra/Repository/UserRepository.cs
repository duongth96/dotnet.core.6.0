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
    public class UserRepository : IUserRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Users> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public UserRepository(SqlCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<Users>();
        }

        public UserRepository()
        {
        }

        public async Task<Users> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await DbSet.ToListAsync();
        } 

        public void Add(Users user)
        {
           DbSet.Add(user);
        }

        public void Update(Users user)
        {
            DbSet.Update(user);
        }

        public void Remove(Users user)
        {
            DbSet.Remove(user);
        }

        public void Dispose()
        {
          Db.Dispose();
        }

        public async Task<IEnumerable<Users>> Query(string keyword, int pagesize, int pageindex, UserQueryParams? UserParams)
        {
            var query = DbSet.Where(x =>
                    x.Email.Contains(keyword) ||
                    x.FullName.Contains(keyword) ||
                    x.LoginID.Contains(keyword)
                );

            if (!string.IsNullOrEmpty(UserParams?.LoginID))
                query = query.Where(c => c.LoginID.Contains(UserParams.LoginID));
            if (!string.IsNullOrEmpty(UserParams?.LoginName))
                query = query.Where(c => (c.LoginName ?? "").Contains(UserParams.LoginName));
            if (!string.IsNullOrEmpty(UserParams?.FullName))
                query = query.Where(c => (c.FullName ?? "").Contains(UserParams.FullName));
            if (!string.IsNullOrEmpty(UserParams?.Email))
                query = query.Where(c => c.Email.Contains(UserParams.Email));
            if (!string.IsNullOrEmpty(UserParams?.Address))
                query = query.Where(c => (c.Address ?? "").Contains(UserParams.Address));
            if (!string.IsNullOrEmpty(UserParams?.Phone))
                query = query.Where(c => (c.Phone ?? "").Contains(UserParams.Phone));
            if (!string.IsNullOrEmpty(UserParams?.Status.ToString()))
                query = query.Where(c => c.Status == UserParams.Status);

            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> Count(string keyword, UserQueryParams? UserParams)
        {
            var query = DbSet.Where(x =>
                    x.Email.Contains(keyword) ||
                    x.FullName.Contains(keyword) ||
                    x.LoginID.Contains(keyword)
                );

            if (!string.IsNullOrEmpty(UserParams?.LoginID))
                query = query.Where(c => c.LoginID.Contains(UserParams.LoginID));
            if (!string.IsNullOrEmpty(UserParams?.LoginName))
                query = query.Where(c => (c.LoginName ?? "").Contains(UserParams.LoginName));
            if (!string.IsNullOrEmpty(UserParams?.FullName))
                query = query.Where(c => (c.FullName ?? "").Contains(UserParams.FullName));
            if (!string.IsNullOrEmpty(UserParams?.Email))
                query = query.Where(c => c.Email.Contains(UserParams.Email));
            if (!string.IsNullOrEmpty(UserParams?.Address))
                query = query.Where(c => (c.Address ?? "").Contains(UserParams.Address));
            if (!string.IsNullOrEmpty(UserParams?.Phone))
                query = query.Where(c => (c.Phone ?? "").Contains(UserParams.Phone));
            if (!string.IsNullOrEmpty(UserParams?.Status.ToString()))
                query = query.Where(c => c.Status == UserParams.Status);

            return await query.CountAsync();
        }
    }
}
