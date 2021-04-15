using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TMessageOrder
    {
        public int CMessageOrderId { get; set; }
        public string CUserName { get; set; }
        public string CMessageOrder { get; set; }
        public int CIsRead { get; set; }
        public DateTime? CSaveTime { get; set; }
    }
}
