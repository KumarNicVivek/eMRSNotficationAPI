using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("T_OtpVerification")]
    public class OtpVerification
    {
        public Int64 Id { get; set; }
        public string UserKey { get; set; }
        public string OTP { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; }
    }
}
