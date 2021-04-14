using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TProductTag
    {
        public TProductTag()
        {
            TProducts = new HashSet<TProduct>();
        }

        public int CProductTagId { get; set; }
        public string CProductTagName { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
