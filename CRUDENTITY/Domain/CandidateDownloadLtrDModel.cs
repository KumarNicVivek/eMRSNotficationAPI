using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class CandidateDownloadLtrDModel
    {
        public Int64 Id { get; set; }
        public int Year { get; set; }
        public string FileName { get; set; }
        public string PostDescription { get; set; }
    }
}
