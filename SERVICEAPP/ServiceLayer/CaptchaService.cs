using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SERVICEAPP.ServiceLayer
{
    public class CaptchaService : ICaptchaService
    {
        private readonly string _captchaFolder;

        public CaptchaService()
        {
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            _captchaFolder = Path.Combine(webRootPath, "captchas");

            if (!Directory.Exists(_captchaFolder))
                Directory.CreateDirectory(_captchaFolder);
        }

        public string GenerateCaptchaImage(out string captchaText)
        {
            var rnd = new Random();
            captchaText = rnd.Next(1000000, 9999999).ToString(); // 7-digit number  

            var fileName = $"{captchaText}_{Guid.NewGuid()}.png";
            var filePath = Path.Combine(_captchaFolder, fileName);

            using (var bmp = new Bitmap(180, 60))
            using (var gfx = Graphics.FromImage(bmp))
            using (var font = new Font("Arial", 28, FontStyle.Bold))
            {
                gfx.Clear(Color.LightYellow);
                gfx.DrawString(captchaText, font, Brushes.DarkBlue, new PointF(20, 10));
                //gfx.DrawLine(Pens.LightGray, 0, 0, 180, 60);
                //gfx.DrawLine(Pens.LightGray, 0, 60, 180, 0);
                bmp.Save(filePath, ImageFormat.Png);
            }

            CleanOldCaptchas(TimeSpan.FromMinutes(10));

            return $"/captchas/{fileName}";
        }

        public bool ValidateCaptcha(string userInput, string sessionCaptcha)
        {
            throw new NotImplementedException();
        }

        private void CleanOldCaptchas(TimeSpan maxAge)
        {
            var files = Directory.GetFiles(_captchaFolder, "*.png");
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                if (DateTime.UtcNow - info.CreationTimeUtc > maxAge)
                {
                    try { System.IO.File.Delete(file); } catch { /* log or ignore */ }
                }
            }
        }
    }
}
