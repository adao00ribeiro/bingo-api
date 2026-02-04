using QRCoder;

namespace bingo_api.src.Services;

public static class PixQrCodeService
{
    public static string GerarQrCodeBase64(string pixCopiaECola)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(pixCopiaECola, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrData);
        byte[] pngBytes = qrCode.GetGraphic(20);
        using var ms = new MemoryStream();

        var base64 = Convert.ToBase64String(pngBytes);
        return $"data:image/png;base64,{base64}";
    }
}
