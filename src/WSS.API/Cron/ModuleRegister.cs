using Quartz;

namespace WSS.API.Cron;

public static class ModuleRegister
{
    [Obsolete("Obsolete")]
    public static void RegisterQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory(opt =>
            {
                opt.AllowDefaultConstructor = true;
            });
            var sendMail = new JobKey("StartSendMailCron", "SendMailGroup");
            
            q.AddJob<StartSendMailCron>(o => o.WithIdentity(sendMail));
            
            q.AddTrigger(opts => opts.ForJob(sendMail)
                .WithIdentity("StartSendMailTrigger")
                //0h0m0s every day
                .WithCronSchedule("0 0 0 ? * *"));
            
            q.InterruptJobsOnShutdown = true;
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddTransient<IJob, StartSendMailCron>();
    }
}