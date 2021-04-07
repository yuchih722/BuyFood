using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class cCategoryController : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

        public IActionResult CategoryList()
        {
           
            var table = from pc in db.TProductCategories
                        select pc;
            List<CCategoryViewModel> list = new List<CCategoryViewModel>();
            foreach (TProductCategory item in table)
            {
                list.Add(new CCategoryViewModel(item));
            }
            return View(list);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CCategoryViewModel p_產品類別新增)
        {
            if (!string.IsNullOrEmpty(p_產品類別新增.CCategoryName))
            {
                db.TProductCategories.Add(p_產品類別新增.allTProductCategory);
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
        public IActionResult Delete(int? id)
        {
            TProductCategory p產品種類移除 = db.TProductCategories.FirstOrDefault(c => c.CProductCategoryId == id);
            if (id != null)
            {
                db.TProductCategories.Remove(p產品種類移除);
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
        public IActionResult EditCategory(int?id)
        {
            if(id != null)
            {

                TProductCategory p_產品種類修改 = db.TProductCategories.FirstOrDefault(n => n.CProductCategoryId == id);
                if (id != null)
                {
                    return View(new CCategoryViewModel(p_產品種類修改));
                }
            }

            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult EditCategory(CCategoryViewModel p_產品種類修改)
        {
            TProductCategory l_product種類被修改 = db.TProductCategories.FirstOrDefault(n => n.CProductCategoryId == p_產品種類修改.CProductCategoryId);

            if(l_product種類被修改 != null){

                l_product種類被修改.CCategoryName = p_產品種類修改.CCategoryName;
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
    }
}
