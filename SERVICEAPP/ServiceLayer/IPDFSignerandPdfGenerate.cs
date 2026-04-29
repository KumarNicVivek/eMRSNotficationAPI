using CRUDENTITY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface IPDFSignerandPdfGenerate
    {
        //public void Sign(string reason, string contact, string location, bool visible);
        public byte[] GeneratePdf(string html);
        public AppointmentLetterDM GetAppointmentLetterData();
    }
}
