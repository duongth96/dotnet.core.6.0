
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EA.Core.Infra
{
    //public class UserEmailEventConsumer : IConsumer<UserEmailEvent>
    //{
    //    private readonly ILogger<UserEmailEventConsumer> _logger;

    //    public UserEmailEventConsumer(ILogger<UserEmailEventConsumer> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public Task Consume(ConsumeContext<UserEmailEvent> context)
    //    {
    //        _logger.LogInformation("UserEmailConsumer:" + JsonConvert.SerializeObject(context.Message));
    //        throw new Exception("Exception UserEmailConsumer");
    //        return Task.CompletedTask;
    //    }
    //}
    
}
