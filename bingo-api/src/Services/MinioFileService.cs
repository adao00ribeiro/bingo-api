
using bingo_api.src.Configurations;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace bingo_api.src.Services;

public class MinioFileService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioSettings _settings;

    public MinioFileService(IMinioClient minioClient, IOptions<MinioSettings> settings)
    {
        _settings = settings.Value;
        _minioClient = minioClient;
    }

    public async Task EnsureBucketExistsAsync()
    {
        bool exists = await _minioClient.BucketExistsAsync(
               new BucketExistsArgs().WithBucket(_settings.DefaultBucketName)
           );

        if (!exists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_settings.DefaultBucketName)
            );
        }
    }

    /// <summary>
    /// Upload de arquivo com prefixo (ex.: rooms/)
    /// </summary>
    public async Task<string> UploadAsync(IFormFile file, string fileName)
    {
        await EnsureBucketExistsAsync();

        string objectName = $"{fileName}";

        using var data = file.OpenReadStream();

        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_settings.DefaultBucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(data.Length)
                .WithContentType(file.ContentType)
        );

        // URL pública (caso MinIO esteja como Site)
        return $"{(_settings.UseSSL ? "https" : "http")}://{_settings.Endpoint}/{_settings.DefaultBucketName}/{objectName}";
    }
    public async Task<string> GetPresignedUrlAsync(string objectName, int expiresSeconds = 3600)
    {
        
        var args = new PresignedGetObjectArgs()
            .WithBucket(_settings.DefaultBucketName)
            .WithObject(objectName)
            .WithExpiry(expiresSeconds);

        return await _minioClient.PresignedGetObjectAsync(args);
    }
    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        MemoryStream ms = new MemoryStream();

        await _minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_settings.DefaultBucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyToAsync(ms))
        );

        ms.Position = 0;
        return ms;
    }
    internal async Task DeleteAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        // URL esperada: http(s)://endpoint/bucket/folder/file.ext
        string baseUrl = $"{(_settings.UseSSL ? "https" : "http")}://{_settings.Endpoint}/{_settings.DefaultBucketName}/";

        if (!url.StartsWith(baseUrl))
            throw new ArgumentException("URL inválida para remoção do MinIO", nameof(url));

        string objectName = url.Replace(baseUrl, "");

        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_settings.DefaultBucketName)
                .WithObject(objectName)
        );
    }
}
