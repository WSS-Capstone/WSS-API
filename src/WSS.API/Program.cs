using System.Reflection;
using L.Core.Config;
using L.Core.Data;
using L.Core.Logging;
using WSS.API.Data.Repositories;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Middleware;

using Microsoft.AspNetCore.Http.Extensions;
using WSS.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*;https://*");
// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.Filters.Add<UserFireFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.RegisterSwaggerModule();
builder.Services.RegisterFireAuth();
builder.Services.RegisterCoreData();
builder.Services.RegisterServiceModule();
builder.Services.RegisterDataRepositories();
// builder.Services.RegisterLogging();
builder.Services.AddFireBaseAsync();
var app = builder.Build();

app
    .UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader())
    .UseDeveloperExceptionPage()
    .UseApplicationSwagger()
    .UseHttpsRedirection();


app.UseHttpsRedirection();

app.UseApplicationSecurity();

app.MapControllers().RequireAuthorization();

app.Run();