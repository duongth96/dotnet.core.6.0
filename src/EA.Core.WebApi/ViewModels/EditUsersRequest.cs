using EA.Core.Application.DTOs;
using EA.NetDevPack.Queries;

namespace EA.Core.Api.ViewModels
{
    public class EditUsersRequest
    {
        public string Sub { get; set; }
        public string? LoginID { get; set; }
        public string? LoginName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
