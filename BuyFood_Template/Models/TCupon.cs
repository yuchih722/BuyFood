using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TCupon
    {
        public TCupon()
        {
            TOrders = new HashSet<TOrder>();
        }

        public int CCuponId { get; set; }
        public int CCuponCategoryId { get; set; }
        public int CMenberId { get; set; }
        public string CDiscountCode { get; set; }
        public DateTime CValidDate { get; set; }
        public DateTime CReceivedTime { get; set; }
        public int CBeUsed { get; set; }

        public virtual TCuponCategory CCuponCategory { get; set; }
        public virtual TMember CMenber { get; set; }
        public virtual ICollection<TOrder> TOrders { get; set; }
    }
}
