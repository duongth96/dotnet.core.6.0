using Quartz;

namespace EA.Core.Job.Jobs
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class HelloJob : IJob
    {
        
        public Task Execute(IJobExecutionContext context)
        {
            var timestamp = DateTime.Now;
                Console.WriteLine($"Hello World! - {timestamp:yyyy-MM-dd HH:mm:ss.fff}");
               return Task.CompletedTask; 
        }


    }
}
