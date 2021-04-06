using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TOrder
    {
        public TOrder()
        {
            TOrderDetails = new HashSet<TOrderDetail>();
        }

        public int COrderId { get; set; }
        public int CMemberId { get; set; }
        public int COrderStatusId { get; set; }
        public int? CCuponId { get; set; }
        public string CArrivedAddress { get; set; }
        public int? CPayTypeId { get; set; }
        public string COrderDate { get; set; }
        public int? CTransportMinute { get; set; }

        public virtual TCupon CCupon { get; set; }
        public virtual TMember CMember { get; set; }
        public virtual TOrderStatus COrderStatus { get; set; }
        public virtual TPayType CPayType { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
    }
}
