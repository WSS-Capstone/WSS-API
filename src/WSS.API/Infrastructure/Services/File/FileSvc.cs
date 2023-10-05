using WSS.API.Infrastructure.Config;
using FileInfo = WSS.API.Infrastructure.Services.File.Models.FileInfo;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.File;

public class FileSvc : IFileSvc
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;
    private const string DIRECTORY = "upload";

    public FileSvc(IHttpContextAccessor contextAccessor, IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }

    public string RootDirectory => this._configuration.GetSection(FileServerConfig.DIRECTORY_SECTION).Value ?? throw new Exception("No Root Directory");


    public async Task<List<FileInfo>> UploadFile(List<IFormFile> files)
    {
        var uploadDirectory = Path.Combine(_hostEnvironment.ContentRootPath, RootDirectory);
        Directory.CreateDirectory(uploadDirectory);
        var tasks = files.Select(iFormFile => UploadTask(iFormFile, uploadDirectory)).ToList();
        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result).ToList();
    }


    private async Task<FileInfo> UploadTask(IFormFile file, string uploadDirectory)
    {
        var indexOfExtension = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
        var extension = file.FileName.Substring(indexOfExtension);
        var filename = Guid.NewGuid() + extension;
        var filePath = Path.Combine(uploadDirectory, filename);

        await using FileStream fs = System.IO.File.Create(filePath);
        await fs.CopyToAsync(fs);
        return new FileInfo()
        {
            Filename = filename,
            Size = SizeConverter(file.Length),
            Type = file.ContentType,
            Link = $"{this._contextAccessor.HttpContext.Request.Host.Value}/{DIRECTORY}/{filename}"
        };
    }

    public void DeleteFile(List<string> listFilename)
    {
        throw new NotImplementedException();
    }

    public string SizeConverter(long bytes)
    {
        throw new NotImplementedException();
    }
}