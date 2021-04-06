using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TProductCategory
    {
        public TProductCategory()
        {
            TProducts = new HashSet<TProduct>();
        }

        public int CProductCategoryId { get; set; }
        public string CCategoryName { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
