using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class StudentAppointmentUploadLogModel
    {
        public string FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public int AppointmentYear { get; set; }
        public int CreatedBy { get; set; }
        public int TotalStudents { get; set; } = 0;
    }
}
