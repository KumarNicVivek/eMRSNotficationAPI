using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface ICaptchaService
    {
        public string GenerateCaptchaImage(out string captchaText);
        string GenerateCaptchaBase64(out string captchaText);
        public bool ValidateCaptcha(string userInput, string sessionCaptcha);
    }
}
