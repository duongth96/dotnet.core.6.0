using EA.Core.Job;
using EA.Core.Job.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using SilkierQuartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSilkierQuartz(options =>
{
    options.VirtualPathRoot = "/jobs";
    options.UseLocalTime = true;
    options.DefaultDateFormat = "yyyy-MM-dd";
    options.DefaultTimeFormat = "HH:mm:ss";
    options.Scheduler= SchedulerConfig.Config(builder.Services, builder.Configuration).Result;
    options.CronExpressionOptions = new CronExpressionDescriptor.Options()
    {
        DayOfWeekStartIndexZero = false 
    };
},authenticationOptions =>
            {
                authenticationOptions.AuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                authenticationOptions.SilkierQuartzClaim = "Silkier";
                authenticationOptions.SilkierQuartzClaimValue = "Quartz";
                authenticationOptions.UserName = "admin";
                authenticationOptions.UserPassword = "123456aA@";
                authenticationOptions.AccessRequirement = SilkierQuartzAuthenticationOptions.SimpleAccessRequirement.AllowOnlyAuthenticated;// AllowAnonymous;// AllowOnlyUsersWithClaim;
            });
 
builder.Services.AddQuartzJob<InjectSampleJob>();
builder.Services.AddQuartzJob<HelloJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

 
app.UseSilkierQuartz();
 
app.Run();
