
using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CCategoryViewModel
    {
        private TProductCategory iv_pCategory = null;
        public CCategoryViewModel(TProductCategory p)
        {
            iv_pCategory = p;
        }
        public TProductCategory allTProductCategory { get { return iv_pCategory; } }

        public CCategoryViewModel()
        {
           iv_pCategory = new TProductCategory();
        }
        public int CProductCategoryId { get {return iv_pCategory.CProductCategoryId; } set {iv_pCategory.CProductCategoryId=value; } }
        [DisplayName("類別名稱")]
        public string CCategoryName { get {return iv_pCategory.CCategoryName; } set {iv_pCategory.CCategoryName=value; } }

    }
}
