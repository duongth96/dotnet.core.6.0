using EA.NetDevPack.Queries;
using Microsoft.AspNetCore.Mvc; 
namespace EA.Core.Api.ViewModels
{
    public class PagingRequest
    {

        [FromQuery(Name = "$inlinecount")]
        public string Inlinecount { get; set; }

        [FromQuery(Name = "$skip")]
        public int Skip { get; set; }
        [FromQuery(Name = "$top")]
        public int Top { get; set; }
    }
}
