using System;
using System.Collections.Generic;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class TMember
    {
        public TMember()
        {
            TBoards = new HashSet<TBoard>();
            TChatRooms = new HashSet<TChatRoom>();
            TCupons = new HashSet<TCupon>();
            TDeposits = new HashSet<TDeposit>();
            TFavoriteLists = new HashSet<TFavoriteList>();
            TLoginRecords = new HashSet<TLoginRecord>();
            TOrders = new HashSet<TOrder>();
        }

        public int CMemberId { get; set; }
        public string CName { get; set; }
        public DateTime? CAge { get; set; }
        public string CEmail { get; set; }
        public string CPassword { get; set; }
        public string CGender { get; set; }
        public string CAddress { get; set; }
        public int? CBlackList { get; set; }
        public decimal? CDeposit { get; set; }
        public string CPhone { get; set; }
        public string CPicture { get; set; }
        public int? CFreezeCount { get; set; }
        public string CReferrerCode { get; set; }
        public int? CReferrerId { get; set; }
        public string CFacebookId { get; set; }
        public DateTime? CRegisteredTime { get; set; }

        public virtual ICollection<TBoard> TBoards { get; set; }
        public virtual ICollection<TChatRoom> TChatRooms { get; set; }
        public virtual ICollection<TCupon> TCupons { get; set; }
        public virtual ICollection<TDeposit> TDeposits { get; set; }
        public virtual ICollection<TFavoriteList> TFavoriteLists { get; set; }
        public virtual ICollection<TLoginRecord> TLoginRecords { get; set; }
        public virtual ICollection<TOrder> TOrders { get; set; }
    }
}
