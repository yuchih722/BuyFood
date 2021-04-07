using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TDeposit
    {
        public int CDepositId { get; set; }
        public int CMemberId { get; set; }
        public DateTime CDepositTime { get; set; }
        public decimal CDepositAmount { get; set; }
        public string CDepositRecordNo { get; set; }

        public virtual TMember CMember { get; set; }
    }
}
