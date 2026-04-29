using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class AppointmentLetterDM
    {
        public DateTime Date { get; set; }
        public string RollNo { get; set; }
        public string CandidateName { get; set; }
        public string IdNo { get; set; }

        public string State { get; set; }
        public string District { get; set; }

        public string PostName { get; set; }

        public string SchoolName { get; set; }
        public string PostingDistrict { get; set; }
        public string PostingState { get; set; }

        public string PayLevel { get; set; }

        public string ReportingEmail { get; set; }

        public string JoiningPlace { get; set; }

        public string ReservedCategory { get; set; }

        public string AppointmentPostName { get; set; }

        public string OfficerName { get; set; }
        public string OfficerDesignation { get; set; }
    }
}
