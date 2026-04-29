using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string username, string password,string role);
        Task<string?> AuthenticateandGenerateTokenAsync(string username, string password, string role);
    }
}
