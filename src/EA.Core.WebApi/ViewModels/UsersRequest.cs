using EA.Core.Application.Queries;
using EA.Core.Domain.Models;
using EA.NetDevPack.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EA.Core.Api.ViewModels
{
    public class UsersRequest
    {
        [FromQuery(Name = "$inlinecount")]
        public string Inlinecount { get; set; }
        [FromQuery(Name = "$skip")]
        public int? Skip { get; set; }
        [FromQuery(Name = "$top")]
        public int? Top { get; set; }
        [FromQuery(Name = "$keyword")]
        public string? Keyword { get; set; }
        [FromQuery(Name = "$loginId")]
        public string? LoginID { get; set; }
        [FromQuery(Name = "$loginName")]
        public string? LoginName { get; set; }
        [FromQuery(Name = "$fullName")]
        public string? FullName { get; set; }
        [FromQuery(Name = "$email")]
        public string? Email { get; set; }
        [FromQuery(Name = "$address")]
        public string? Address { get; set; }
        [FromQuery(Name = "$phone")]
        public string? Phone { get; set; }
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        public int PageSize { get { return Top ?? 12; } }
        public int PageIndex { get { return ((Skip ?? 1) / PageSize) + 1; } }
        public UserQueryParams ToBaseQuery() => new UserQueryParams
        {
            LoginID = LoginID,
            LoginName = LoginName,
            FullName = FullName,
            Email = Email,
            Address = Address,
            Phone = Phone,
            Status = Status
        };
    }
}
