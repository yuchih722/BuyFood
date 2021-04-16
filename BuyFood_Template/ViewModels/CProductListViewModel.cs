
using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CProductListViewModel
    {

        private TProduct iv_product = null;
        private TProductCategory iv_productCategory = null;
        private TIsOnSale iv_isOnSale = null;

        public CProductListViewModel(TProduct p, TProductCategory c, TIsOnSale s)
        {
            iv_product = p;
            iv_productCategory = c;
            iv_isOnSale = s;
        }
        public TProduct allProduct { get { return iv_product; } }
        public TProductCategory allproductCategory { get { return iv_productCategory; } }
        public TIsOnSale allisOnSale { get { return iv_isOnSale; } }

        //新增產品使用
        public CProductListViewModel()
        {
            iv_product = new TProduct();
            iv_productCategory = new TProductCategory();
            iv_isOnSale = new TIsOnSale();
        }
        public int CProductId { get { return iv_product.CProductId; } set { iv_product.CProductId = value; } }
        [DisplayName("販售時段")]
        public int? CEatTimeId { get { return iv_product.CEatTimeId; } set { iv_product.CEatTimeId = value; } }
        [DisplayName("產品類別")]
        public int CCategoryId { get { return iv_product.CCategoryId; } set { iv_product.CCategoryId = value; } }
        [DisplayName("產品狀況")]
        public int CIsOnSaleId { get { return iv_product.CIsOnSaleId; } set { iv_product.CIsOnSaleId = value; } }
        [DisplayName("產品名稱")]
        public string CProductName { get { return iv_product.CProductName; } set { iv_product.CProductName = value; } }
        [DisplayName("產品價格")]
        public decimal? CPrice { get { return iv_product.CPrice; } set { iv_product.CPrice = value; } }
        [DisplayName("產品庫存")]
        public int? CQuantity { get { return iv_product.CQuantity; } set { iv_product.CQuantity = value; } }
        public int? CFinishedTime { get { return iv_product.CFinishedTime; } set { iv_product.CFinishedTime = value; } }
        [DisplayName("產品描述")]
        public string CDescription { get { return iv_product.CDescription; } set { iv_product.CDescription = value; } }

        [DisplayName("產品照片")]
        public string CPicture { get { return iv_product.CPicture; } set { iv_product.CPicture = value; } }

        [DisplayName("販售時段早")]
        public int? CIsBreakFast { get { return iv_product.CIsBreakFast; } set { iv_product.CIsBreakFast = value; } }
        [DisplayName("販售時段午")]
        public int? CIsLunch { get { return iv_product.CIsLunch; } set { iv_product.CIsLunch = value; } }
        [DisplayName("販售時段晚")]
        public int? CIsDinner { get { return iv_product.CIsDinner; } set { iv_product.CIsDinner = value; } }
        [DisplayName("產品標籤")]
        public int? CProductTagId { get { return iv_product.CProductTagId; } set { iv_product.CProductTagId = value; } }
        [DisplayName("庫存保險量")]
        public int? CQuantityControl { get {return iv_product.CQuantityControl; } set {iv_product.CQuantityControl=value; } }

      [DisplayName("產品類別")]
        public string CCategoryName { get {return iv_productCategory.CCategoryName; } set {iv_productCategory.CCategoryName=value; } }
        public string CStatusName { get {return iv_isOnSale.CStatusName; } set {iv_isOnSale.CStatusName=value; } }
    }
}
