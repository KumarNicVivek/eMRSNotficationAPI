using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Keyless]
    public class NotificationDataEntity
    {
        public Int32 LinkTempId { get; set; }

        public string? Title { get; set; }

        public string? Details { get; set; }

        public Int32? LinkLevel { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? PublishOn { get; set; }
    }
}
