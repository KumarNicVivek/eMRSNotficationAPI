using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("T_StudentAppointmentUploadLog")]
    public class StudentAppointmentUploadLog
    {
        [Key]
        public Int64 Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public int AppointmentYear { get; set; }
        public Int64 CreatedBy { get; set; }
    }
}
