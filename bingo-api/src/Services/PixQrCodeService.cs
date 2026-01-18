using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace bingo_api.src.Services;

public static  class PixQrCodeService
{
     public static string GerarQrCodeBase64(string pixCopiaECola)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(pixCopiaECola, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new QRCode(qrData);
            using Bitmap qrImage = qrCode.GetGraphic(20);
            using var ms = new MemoryStream();

            qrImage.Save(ms, ImageFormat.Png);
            
              var base64 = Convert.ToBase64String(ms.ToArray());
              return $"data:image/png;base64,{base64}";
        }
}
