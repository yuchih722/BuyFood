using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TIsOnSale
    {
        public TIsOnSale()
        {
            TProducts = new HashSet<TProduct>();
        }

        public int CIsOnSaleId { get; set; }
        public string CStatusName { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
