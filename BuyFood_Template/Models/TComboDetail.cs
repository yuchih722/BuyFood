using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TComboDetail
    {
        public int CComboDetailId { get; set; }
        public int CComboId { get; set; }
        public int CProductId { get; set; }

        public virtual TCombo CCombo { get; set; }
        public virtual TProduct CProduct { get; set; }
    }
}
