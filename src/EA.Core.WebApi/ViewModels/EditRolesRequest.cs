using EA.Core.Application.DTOs;
using EA.NetDevPack.Queries;

namespace EA.Core.Api.ViewModels
{
    public class RolesUsersRequest
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
