using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface IVisitorService
    {
        Task<int> TrackVisitorAsync(HttpContext context);
    }
}
