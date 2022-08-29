using EA.NetDevPack.Queries;

namespace EA.Core.Api.ViewModels
{
    public class AddUsersRequest
    {
        public string LoginID { get; set; }
        public string? LoginName { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        public int? Status { get; set; }
    }
}
