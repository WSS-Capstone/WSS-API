using WSS.API.Infrastructure.Config;
using FileInfo = WSS.API.Infrastructure.Services.File.Models.FileInfo;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.File;

public class FileSvc : IFileSvc
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;

    public FileSvc(IHttpContextAccessor contextAccessor, IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }

    private string RootDirectory => this._configuration.GetSection(FileServerConfig.DIRECTORY_SECTION).Value ??
                                    throw new Exception("No Root Directory");

    private string UploadDirectory => Path.Combine(_hostEnvironment.ContentRootPath, RootDirectory);

    public async Task<List<FileInfo>> UploadFile(List<IFormFile> files)
    {
        Directory.CreateDirectory(UploadDirectory);
        var tasks = files.Select(iFormFile => UploadTask(iFormFile, UploadDirectory)).ToList();
        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result).ToList();
    }


    private async Task<FileInfo> UploadTask(IFormFile file, string uploadDirectory)
    {
        var indexOfExtension = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
        var extension = file.FileName.Substring(indexOfExtension);
        var filename = Guid.NewGuid() + extension;
        var filePath = Path.Combine(uploadDirectory, filename);

        using (FileStream fs = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(fs);
        }
        return new FileInfo()
        {
            Filename = filename,
            Size = SizeConverter(file.Length),
            Type = file.ContentType,
            Link = $"https://{this._contextAccessor.HttpContext.Request.Host.Value}/{RootDirectory}/{filename}"
        };
    }

    public void DeleteFile(List<string> listFilename)
    {
        foreach (var path in listFilename.Select(filename => UploadDirectory + "/" + filename))
        {
            System.IO.File.Delete(path);
        }
    }

    public string SizeConverter(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        switch (fileSize)
        {
            case var _ when fileSize < kilobyte:
                return $"Less then 1KB";
            case var _ when fileSize < megabyte:
                return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
            case var _ when fileSize < gigabyte:
                return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
            case var _ when fileSize >= gigabyte:
                return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
            default:
                return "n/a";
        }
    }
}