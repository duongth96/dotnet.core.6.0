using EA.Core.Domain.Models;
using EA.NetDevPack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Domain.Interfaces
{
    public interface IUserRepository : IRepository<Users>
    {  
        Task<IEnumerable<Users>> Query(string keyword, int pagesize, int pageindex, UserQueryParams? userQueryParams);
        Task<int> Count(string keyword, UserQueryParams? userQueryParams);
    } 
}
