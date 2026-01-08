using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;


[Authorize]
[ApiVersion("1.0")]
public class MediaController : ApiControllerBase
{
    private readonly MinioFileService _minioService;

    public MediaController(MinioFileService minioService)
    {
        _minioService = minioService;
    }

    /// <summary>
    /// Download do arquivo pelo nome completo (folder/NomeDoArquivo.ext)
    /// Exemplo de chamada: GET /api/media/download?filePath=rooms/minhaImagem.jpg
    /// </summary>
    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest("fileName inválido!");

        Stream fileStream = await _minioService.DownloadFileAsync(fileName);

        fileStream.Position = 0;

        // Tipo genérico de imagem (pode ajustar para outros tipos)
        return File(fileStream, "application/octet-stream", Path.GetFileName(fileName));
    }
    [HttpGet("presigned/{fileName}")]
    public async Task<ActionResult<string>> GetPresignedUrl(string fileName)
    {

        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest("Nome do arquivo inválido.");

        string signedUrl = await _minioService.GetPresignedUrlAsync(fileName);

        return Ok(signedUrl);
    }
}
