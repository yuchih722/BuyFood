using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TEatPeriod
    {
        public TEatPeriod()
        {
            TProducts = new HashSet<TProduct>();
        }

        public int CEatPeriodId { get; set; }
        public string CEatPeriodName { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
