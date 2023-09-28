using System.Reflection;
using L.Core.Config;
using L.Core.Data;
using L.Core.Logging;
using WSS.API.Data.Repositories;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Middleware;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WSS.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*;https://*");
// Add services to the container.
builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddControllers(config =>
{
    config.Filters.Add<UserFireFilter>();
}).AddNewtonsoftJson(o =>
{
    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
    o.SerializerSettings.ContractResolver = new DefaultContractResolver()
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };
                
    o.SerializerSettings.Converters.Add(new StringEnumConverter()
    {
        AllowIntegerValues = true
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.RegisterSwaggerModule();
builder.Services.RegisterFireAuth();
builder.Services.RegisterCoreData();
builder.Services.RegisterServiceModule();
builder.Services.RegisterDataRepositories();
builder.Services.RegisterLogging();
builder.Services.AddFireBaseAsync();
var app = builder.Build();

Directory.CreateDirectory("upload");
app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "upload")),
    RequestPath = "/upload",
    EnableDirectoryBrowsing = true,
    StaticFileOptions = { OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    }}
});

// Configure the HTTP request pipeline.
app.UseCors(cor => cor
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials())
    .UseDeveloperExceptionPage()
    .UseApplicationSwagger()
    .UseHttpsRedirection();


app.UseHttpsRedirection();

app.UseApplicationSecurity();

app.MapControllers().RequireAuthorization();

app.Run();