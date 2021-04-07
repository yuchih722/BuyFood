
using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{

    public class OrderDetailsViewModel
    {

        private TOrderDetail iv_orderDetails = null;
        private TMember iv_member = null;
        private TOrderStatus iv_orderStatus = null;
        private TProduct iv_product = null;


        public TOrderDetail orderDetail { get{ return iv_orderDetails; } }
        public TMember member { get { return iv_member; } }
        public TOrderStatus orderStatus { get{ return iv_orderStatus; } }
        public TProduct product { get { return iv_product; } }

        public OrderDetailsViewModel(TOrderDetail orderDetail, TMember member, TOrderStatus orderStatus, TProduct product)
        {
            orderDetail = iv_orderDetails;
            member = iv_member;
            orderStatus = iv_orderStatus;
            product = iv_product;
        }

        public OrderDetailsViewModel()
        {
            iv_orderDetails = new TOrderDetail();
            iv_member = new TMember();
            iv_orderStatus = new TOrderStatus();
            iv_product = new TProduct();
        }


        //public int COrderDetailId 
        [DisplayName("產品編號")]
        public int CProductId { get {return iv_orderDetails.CProductId; } set {iv_orderDetails.CProductId=value; } }
        [DisplayName("訂單編號")]
        public int COrderId { get {return iv_orderDetails.COrderId; } set {iv_orderDetails.COrderId=value; } }
        [DisplayName("數量")]
        public int? CQuantity { get {return iv_orderDetails.CQuantity; } set {iv_orderDetails.CQuantity=value; } }
        [DisplayName("價格")]
        public decimal? CPriceAtTheTime { get {return iv_orderDetails.CPriceAtTheTime; } set {iv_orderDetails.CPriceAtTheTime=value; } }

        //public virtual TOrder COrder { get; set; }
        //public virtual TProduct CProduct { get; set; }


        [DisplayName("會員姓名")]
        public string CName  { get {return iv_member.CName; } set {iv_member.CName=value; } }

        [DisplayName("運送狀態")]
        public string COrderStatusName { get {return iv_orderStatus.COrderStatusName; } set {iv_orderStatus.COrderStatusName=value; } }
        [DisplayName("產品名稱")]
        public string CProductName { get {return iv_product.CProductName; } set {iv_product.CProductName=value; } }
    }
}





