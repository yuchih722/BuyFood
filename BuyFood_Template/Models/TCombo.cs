using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TCombo
    {
        public TCombo()
        {
            TComboDetails = new HashSet<TComboDetail>();
        }

        public int CComboId { get; set; }
        public int CMemberId { get; set; }
        public string CComboName { get; set; }

        public virtual ICollection<TComboDetail> TComboDetails { get; set; }
    }
}
