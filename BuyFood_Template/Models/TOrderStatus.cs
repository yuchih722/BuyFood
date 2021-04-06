using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TOrderStatus
    {
        public TOrderStatus()
        {
            TOrders = new HashSet<TOrder>();
        }

        public int COrderStatusId { get; set; }
        public string COrderStatusName { get; set; }

        public virtual ICollection<TOrder> TOrders { get; set; }
    }
}
