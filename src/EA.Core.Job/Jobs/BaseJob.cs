using System;
using System.Threading.Tasks;
using Quartz;

namespace EA.Core.Job
{
    public abstract class BaseJob : IJob
    {
        protected TResult GetJobDataMap<TResult>(JobDataMap jobDataMap,
            string key,
            TResult @default = default(TResult))
        {
            if (!jobDataMap.ContainsKey(key))
            {
                return @default;
            }

            var value = jobDataMap[key];

            try
            {
                return ChangeType<TResult>(value);
            }
            catch
            {
                return @default;
            }
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

        public abstract Task Execute(IJobExecutionContext context);
    }
}