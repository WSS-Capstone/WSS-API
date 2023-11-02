using Microsoft.AspNetCore.Authorization;
using WSS.API.Infrastructure.Services.File;

namespace WSS.API.Controllers;
[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[ApiVersion("3")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FileController : BaseController
{
    private readonly IFileSvc _fileSvc;
    /// <inheritdoc />
    public FileController(IFileSvc fileSvc, IMediator mediator) : base(mediator)
    {
        _fileSvc = fileSvc;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files,
        CancellationToken cancellationToken = default)
    {
        var result = await _fileSvc.UploadFile(files);

        return Ok(result);
    }
}