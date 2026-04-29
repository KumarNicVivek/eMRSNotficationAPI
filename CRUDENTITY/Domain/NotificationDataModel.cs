using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class NotificationDataModel
    {       
        public int LinkTempId { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public int? LinkLevel { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? PublishOn { get; set; }
        
    }
}
