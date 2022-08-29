using EA.Core.Domain.Interfaces;
using EA.Core.Domain.Models;
using MediatR;
using EA.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA.Core.Application.DTOs;

namespace EA.Core.Application.Queries
{
    public class RoleQueryAll : IQuery<IEnumerable<RoleDto>>
    {
        public RoleQueryAll()
        {
        }
    }
    public class RoleQueryById : IQuery<RoleDto>
    {
        public RoleQueryById()
        {
        }

        public RoleQueryById(Guid roleId)
        {
            RoleId = roleId;
        }

        public Guid RoleId { get; set; }
    }
    public class RolePagingQuery : ListQuery, IQuery<PagingResponse<RoleDto>>
    {
        public RolePagingQuery(string keyword, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
        }

        public RolePagingQuery(string keyword, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
        }

        public string Keyword { get; set; }
    }
    public class RoleQueryHandler : IQueryHandler<RoleQueryAll, IEnumerable<RoleDto>>, IQueryHandler<RoleQueryById, RoleDto>, IQueryHandler<RolePagingQuery, PagingResponse<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        public RoleQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<RoleDto>> Handle(RoleQueryAll request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAll();
            var result = roles.Select(x=> new RoleDto()
            {
                Id = x.Id,
                FullName = x.FullName,
                Role = x.Role,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                ModifiedDate = x.ModifiedDate,
                ModifiedBy = x.ModifiedBy
            });

            return result;
        }

        public async Task<RoleDto> Handle(RoleQueryById request, CancellationToken cancellationToken)
        {

            var x = await _roleRepository.GetById(request.RoleId);
            if (x == null)
            {
                return null;
            }

            var result = new RoleDto()
            {
                Id = x.Id,
                FullName = x.FullName,
                Role = x.Role,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                ModifiedDate = x.ModifiedDate,
                ModifiedBy = x.ModifiedBy
            };

            return result;
        }
        public async Task<PagingResponse<RoleDto>> Handle(RolePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<RoleDto>();
            var count = await _roleRepository.Count(request.Keyword);
            var roles = await _roleRepository.Query(request.Keyword, request.PageSize, request.PageIndex);
            var data = roles.Select(x => new RoleDto()
            {
                Id = x.Id,
                FullName = x.FullName,
                Role = x.Role,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                ModifiedDate = x.ModifiedDate,
                ModifiedBy = x.ModifiedBy
            });

            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();

            return response;
        }
        public void Dispose()
        {
            _roleRepository.Dispose();
        }
    }
}
