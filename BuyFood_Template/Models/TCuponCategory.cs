using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TCuponCategory
    {
        public TCuponCategory()
        {
            TCupons = new HashSet<TCupon>();
        }

        public int CCuponCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? CCutPrice { get; set; }

        public virtual ICollection<TCupon> TCupons { get; set; }
    }
}
