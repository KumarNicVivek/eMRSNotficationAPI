using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class ProgressModel
    {
        public int Total;
        public int Processed;
        public int Running;
        public DateTime StartTime;
        public bool IsFinalizing;
    }
}
