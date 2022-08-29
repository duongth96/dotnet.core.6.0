using EA.Core.Domain.Models;
using EA.NetDevPack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Domain.Interfaces
{
    public interface IRoleRepository : IRepository<Roles>
    {
        Task<IEnumerable<Roles>> Query(string keyword, int pagesize, int pageindex);
        Task<int> Count(string keyword);
    }
}
