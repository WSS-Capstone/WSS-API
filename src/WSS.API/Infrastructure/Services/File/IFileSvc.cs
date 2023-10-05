using FileInfo = WSS.API.Infrastructure.Services.File.Models.FileInfo;

namespace WSS.API.Infrastructure.Services.File;

public interface IFileSvc
{
    Task<List<FileInfo>> UploadFile(List<IFormFile> files);

    void DeleteFile(List<string> listFilename);
    string SizeConverter(long bytes);
}