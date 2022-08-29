using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Job.Jobs
{
    public class InjectSampleJob : IJob
    {
        
        public InjectSampleJob()
        {
             
        }
        public Task Execute(IJobExecutionContext context)
        {
            var key = "TEXT";
            var text = "";
            var jobDataMap = context.Trigger.JobDataMap;
            if (jobDataMap.ContainsKey(key))
            {
                text= ChangeType<string>(jobDataMap[key]); ; 
            }

           
            Console.WriteLine("InjectSampleJob: "+ text);
            return Task.CompletedTask;
        }
        protected TResult ChangeType<TResult>(object value)
        {
            var type = typeof(TResult);

            if (type.IsGenericType
                && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(TResult);
                }

                type = Nullable.GetUnderlyingType(type);
            }

            return (TResult)Convert.ChangeType(value, type);
        }

    }
}
