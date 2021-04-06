using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TOrderDetail
    {
        public int COrderDetailId { get; set; }
        public int CProductId { get; set; }
        public int COrderId { get; set; }
        public int? CQuantity { get; set; }
        public decimal? CPriceAtTheTime { get; set; }
        public int? CScores { get; set; }
        public string CReview { get; set; }
        public int? CFeedBackStatus { get; set; }

        public virtual TOrder COrder { get; set; }
        public virtual TProduct CProduct { get; set; }
    }
}
