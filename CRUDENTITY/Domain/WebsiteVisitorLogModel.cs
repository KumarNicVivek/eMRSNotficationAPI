using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class WebsiteVisitorLogModel
    {
        public Int64 Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
