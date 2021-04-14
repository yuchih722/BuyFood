using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TProduct
    {
        public TProduct()
        {
            TBoards = new HashSet<TBoard>();
            TComboDetails = new HashSet<TComboDetail>();
            TNotifyLists = new HashSet<TNotifyList>();
            TOrderDetails = new HashSet<TOrderDetail>();
        }

        public int CProductId { get; set; }
        public int? CEatTimeId { get; set; }
        public int CCategoryId { get; set; }
        public int CIsOnSaleId { get; set; }
        public string CProductName { get; set; }
        public decimal? CPrice { get; set; }
        public int? CQuantity { get; set; }
        public string CPicture { get; set; }
        public int? CFinishedTime { get; set; }
        public string CDescription { get; set; }
        public int? CIsBreakFast { get; set; }
        public int? CIsLunch { get; set; }
        public int? CIsDinner { get; set; }
        public int? CProductTagId { get; set; }

        public virtual TProductCategory CCategory { get; set; }
        public virtual TProductTag CProductTag { get; set; }
        public virtual ICollection<TBoard> TBoards { get; set; }
        public virtual ICollection<TComboDetail> TComboDetails { get; set; }
        public virtual ICollection<TNotifyList> TNotifyLists { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
    }
}
