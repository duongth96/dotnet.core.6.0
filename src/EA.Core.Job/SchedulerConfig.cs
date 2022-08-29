
using EA.Core.Job.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace EA.Core.Job
{
    public static class SchedulerConfig
    {
        public static async Task<IScheduler> Config(IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringConfig =configuration.GetConnectionString("DbJobConnectionString"); 
            var properties = new NameValueCollection
            {
                [StdSchedulerFactory.PropertySchedulerInstanceName] = "Core Scheduler",
                [StdSchedulerFactory.PropertySchedulerInstanceId] = "JobSchedulerId",

                [StdSchedulerFactory.PropertyThreadPoolType] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "10",
                ["quartz.threadPool.threadPriority"] = "Normal",

                ["quartz.jobStore.misfireThreshold"] = "60000",
                [StdSchedulerFactory.PropertyJobStoreType] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "true",
                ["quartz.jobStore.dataSource"] = "default",
                [StdSchedulerFactory.PropertyTablePrefix] = "QRTZ_",
                [StdSchedulerFactory.PropertyJobStoreLockHandlerType] = "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz",

               ["quartz.dataSource.default.connectionString"] = connectionStringConfig,
                ["quartz.dataSource.default.provider"] = "SqlServer",
                ["quartz.serializer.type"] = "json",

                ["quartz.plugin.recentHistory.type"] = "Quartz.Plugins.RecentHistory.ExecutionHistoryPlugin, Quartz.Plugins.RecentHistory",
                ["quartz.plugin.recentHistory.storeType"] = "Quartz.Plugins.RecentHistory.Impl.InProcExecutionHistoryStore, Quartz.Plugins.RecentHistory"
            };

            var stdSchedulerFactory = new StdSchedulerFactory(properties);
//#if DEBUG
 //           var scheduler = await StdSchedulerFactory.GetDefaultScheduler(); //Local
//#else
            var scheduler = await stdSchedulerFactory.GetScheduler(); //Production
//#endif

            //scheduler.JobFactory = new JobFactory(services.BuildServiceProvider());

            await ConfigJobs(scheduler);

            await scheduler.Start();

            //_ = AutoResumePausedTrigger(scheduler);

            return scheduler;
        }

        private static async Task AutoResumePausedTrigger(IScheduler scheduler)
        {
            while (true)
            {
                try
                {
                    var groupNames = await scheduler.GetTriggerGroupNames();

                    foreach (var item in groupNames)
                    {
                        var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(item));

                        foreach (var triggerKey in triggerKeys)
                        {
                            var triggerState = await scheduler.GetTriggerState(triggerKey);

                            if (triggerState != TriggerState.Paused)
                            {
                                continue;
                            }

                            try
                            {
                                await scheduler.ResumeTrigger(triggerKey);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }

                await Task.Delay(5000);
            }
        }

     
        private static async Task ConfigJobs(IScheduler scheduler)
        {
            await ConfigHelloJob(scheduler); 
            await ConfigInjectSampleJob(scheduler);
        }


        private static async Task ConfigHelloJob(IScheduler scheduler)
        {
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity("HelloJob")
                .StoreDurably()
                .Build();

            await scheduler.AddJob(job, true);
        }
        private static async Task ConfigInjectSampleJob(IScheduler scheduler)
        {
            var job = JobBuilder.Create<InjectSampleJob>()
                .WithIdentity("InjectSampleJob")
                .StoreDurably()
                .Build();

            await scheduler.AddJob(job, true);
        }
    }
}