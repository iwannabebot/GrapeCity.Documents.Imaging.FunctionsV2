using System;
using System.IO;
using System.Drawing;
using GrapeCity.Documents.Drawing;
using GrapeCity.Documents.Text;
using GrapeCity.Documents.Imaging;
using System.Configuration;

namespace GrapeCity.Documents.Imaging.FunctionsV2
{
    public class GcImagingOperations
    {
        public static byte[] GetConvertedImage(byte[] str)
        {
            GcBitmap.SetLicenseKey(System.Environment.GetEnvironmentVariable("GcImagingLicenseKey", EnvironmentVariableTarget.Process));

            using (var bmp = new GcBitmap())
            {
                bmp.Load(str);
                // Add watermark
                var newImg = new GcBitmap();
                newImg.Load(str);
                using (var g = bmp.CreateGraphics(Color.White))
                {
                    g.DrawImage(
                        Image.FromGcBitmap(newImg, true),
                        new RectangleF(0, 0, bmp.Width, bmp.Height),
                        null,
                        ImageAlign.Default
                        );
                    g.DrawString("DOCUMENT", new TextFormat
                    {
                        FontSize = 22,
                        ForeColor = Color.FromArgb(128, Color.Yellow),
                        Font = FontCollection.SystemFonts.DefaultFont
                    },
                    new RectangleF(0, 0, bmp.Width, bmp.Height),
                    TextAlignment.Center, ParagraphAlignment.Center, false);
                }
                // GcBitmap.SetLicenseKey("");
                //  Convert to grayscale
                bmp.ApplyEffect(GrayscaleEffect.Get(GrayscaleStandard.BT601));
                //  Resize to thumbnail
                var resizedImage = bmp.Resize(100, 100, InterpolationMode.NearestNeighbor);
                using (MemoryStream m = new MemoryStream())
                {
                    resizedImage.SaveAsPng(m);
                    m.Position = 0;

                    byte[] _buffer = new byte[16 * 1024];
                    using (MemoryStream _ms = new MemoryStream())
                    {
                        int _read;
                        while ((_read = m.Read(_buffer, 0, _buffer.Length)) > 0)
                        {
                            _ms.Write(_buffer, 0, _read);
                        }
                        return _ms.ToArray();
                    }
                }
            }


        }
    }
}
