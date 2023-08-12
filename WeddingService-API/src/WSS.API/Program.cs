using WSS.API.Infrastructure.Config;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*;https://*");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app
    .UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader())
    .UseDeveloperExceptionPage()
    .UseApplicationSwagger()
    .UseHttpsRedirection()
    ;

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();