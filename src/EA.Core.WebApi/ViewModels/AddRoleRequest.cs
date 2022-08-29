using EA.NetDevPack.Queries;

namespace EA.Core.Api.ViewModels
{
    public class AddRoleRequest
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
    }
}
