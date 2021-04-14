using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TNotifyList
    {
        public int CNotifyId { get; set; }
        public int CProductId { get; set; }
        public string CEmail { get; set; }
        public int CIsSend { get; set; }

        public virtual TProduct CProduct { get; set; }
    }
}
