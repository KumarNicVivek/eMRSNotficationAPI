using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class StudentAppointmentModel
    {
        public Int64 Id { get; set; }
        public string UniqueId { get; set; }
        public string RollNo { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; }
        public string PwBD { get; set; }
        public string Designation { get; set; }

        public string CandidateState { get; set; }
        public string CandidateDistrict { get; set; }

        public string SchoolState { get; set; }
        public string HomeState { get; set; }

        public string SchoolName { get; set; }
        public string SchoolBillUnitNo { get; set; }
        public string SchoolDistrict { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string PayLevel { get; set; }
        public decimal? BasicPay { get; set; }
        public int AppointmentYear { get; set; }
        public int ISPDFGenerate { get; set; }
        public int ISPDFSigned { get; set; }
    }
}
