using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TPayType
    {
        public TPayType()
        {
            TOrders = new HashSet<TOrder>();
        }

        public int CPayTypeId { get; set; }
        public string CPayTypeName { get; set; }

        public virtual ICollection<TOrder> TOrders { get; set; }
    }
}
