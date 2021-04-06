using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TActivity
    {
        public int CActivityId { get; set; }
        public string CActivityName { get; set; }
        public string CDescription { get; set; }
        public string CPicture { get; set; }
        public string CLink { get; set; }
        public int? CRank { get; set; }
        public int? CStatus { get; set; }
        public string CTime { get; set; }
    }
}
