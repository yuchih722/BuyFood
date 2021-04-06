using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TBoard
    {
        public int CBoardId { get; set; }
        public int? CProductId { get; set; }
        public int CMemberId { get; set; }
        public decimal? CGrades { get; set; }
        public string CContent { get; set; }
        public string CPicture { get; set; }
        public DateTime? CBoardTime { get; set; }
        public string CBordStatus { get; set; }

        public virtual TMember CMember { get; set; }
        public virtual TProduct CProduct { get; set; }
    }
}
