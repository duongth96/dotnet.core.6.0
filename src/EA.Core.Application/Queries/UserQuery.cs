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
    public class UserQueryAll : IQuery<IEnumerable<UserDto>>
    {
        public UserQueryAll()
        {
        }
    }
    public class UserQueryById : IQuery<UserDto>
    {
        public UserQueryById()
        {
        }

        public UserQueryById(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
    public class UserPagingQuery : ListQuery, IQuery<PagingResponse<UserDto>>
    {
        public UserPagingQuery(string keyword, int pageSize, int pageIndex, UserQueryParams? userQueryParams) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            UserParams = userQueryParams;
        }

        public UserPagingQuery(string keyword, int pageSize, int pageIndex, UserQueryParams? userQueryParams, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
            UserParams = userQueryParams;
        }

        public string Keyword { get; set; }
        public UserQueryParams? UserParams { get; set; }
    }
    public class UserQueryHandler : IQueryHandler<UserQueryAll, IEnumerable<UserDto>>, IQueryHandler<UserQueryById, UserDto>, IQueryHandler<UserPagingQuery, PagingResponse<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        public UserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDto>> Handle(UserQueryAll request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll();
            var result = users.Select(x=> new UserDto()
            {
                Sub = x.Sub,
                LoginID = x.LoginID,
                LoginName = x.LoginName,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Status = x.Status,
                Address = x.Address,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                ModifiedDate = x.ModifiedDate,
                ModifiedBy = x.ModifiedBy
            });
            return result;
        }

        public async Task<UserDto> Handle(UserQueryById request, CancellationToken cancellationToken)
        {

            var x = await _userRepository.GetById(request.UserId);
            if (x == null)
            {
                return null;
            }

            var result = new UserDto()
            {
                Sub = x.Sub,
                LoginID = x.LoginID,
                LoginName = x.LoginName,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Status = x.Status,
                Address = x.Address,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                ModifiedDate = x.ModifiedDate,
                ModifiedBy = x.ModifiedBy
            };
            return result;
        }
        public async Task<PagingResponse<UserDto>> Handle(UserPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<UserDto>();
            var count = await _userRepository.Count(request.Keyword, request?.UserParams);
            var users = await _userRepository.Query(request.Keyword, request.PageSize, request.PageIndex, request?.UserParams);
            var data = users.Select(x => new UserDto()
            {
                Sub = x.Sub,
                LoginID = x.LoginID,
                LoginName = x.LoginName,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Status = x.Status,
                Address = x.Address,
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
            _userRepository.Dispose();
        }
    }
}
