using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TFavoriteList
    {
        public int CFavorId { get; set; }
        public int CMemberId { get; set; }
        public int CProductId { get; set; }

        public virtual TMember CMember { get; set; }
        public virtual TProduct CProduct { get; set; }
    }
}
