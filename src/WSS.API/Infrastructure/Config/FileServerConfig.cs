using Microsoft.Extensions.FileProviders;

namespace WSS.API.Infrastructure.Config;

public static class FileServerConfig
{
    public static string DIRECTORY_SECTION = "RootFileDirectory";
    
    public static void RegisterFileServer(this WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
    {
        var rootDirectory = configuration.GetSection(DIRECTORY_SECTION).Value;

        if (rootDirectory == null)
        {
            throw new Exception("No Directory File");
        }
        
        Directory.CreateDirectory(rootDirectory);
        app.UseFileServer(new FileServerOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, rootDirectory)),
            RequestPath = "/" + rootDirectory,
            EnableDirectoryBrowsing = true,
            StaticFileOptions = { OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            }}
        });
        
    }
}