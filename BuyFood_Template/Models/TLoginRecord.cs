using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TLoginRecord
    {
        public int CLoginRecordId { get; set; }
        public int CMemberId { get; set; }
        public DateTime? CLoginTime { get; set; }
        public string CIsLoginSuccess { get; set; }
        public string CIp { get; set; }

        public virtual TMember CMember { get; set; }
    }
}
