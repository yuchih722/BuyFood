using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class yuOrderViewModel
    {
        public yuOrderViewModel()
        {
            iv_Order = new TOrder();
        }
        private TOrder iv_Order = null;
        public TOrder Order { get { return iv_Order; } }

        public yuOrderViewModel(TOrder p)
        {
            iv_Order = p;
        }

        public yuOrderViewModel(TOrder p, TCupon pCupon, TMember pMember, TOrderStatus pOrderStatus)
        {
            iv_Order = p;
            CCupon = pCupon;
            CMember = pMember;
            COrderStatus = pOrderStatus;
        }
        [DisplayName("訂單編號")]
        public int COrderId { get; set; }
        [DisplayName("會員編號")]
        public int CMemberId { get; set; }
        [DisplayName("狀態")]
        public int COrderStatusId { get; set; }
        [DisplayName("訂單折價券編號")]
        public int? CCuponId { get; set; }
        [DisplayName("訂單地址")]
        public string CArrivedAddress { get; set; }
        [DisplayName("訂單時間")]
        public DateTime? COrderDate { get; set; }
        public virtual TCupon CCupon { get; set; }
        public virtual TMember CMember { get; set; }
        public virtual TOrderStatus COrderStatus { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
        }
}
