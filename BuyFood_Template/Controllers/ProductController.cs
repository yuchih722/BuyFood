using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;

namespace prjBuyFood.Controllers
{
    public class ProductController : Controller
    {
        private IHostingEnvironment iv_host;

        public ProductController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        public IActionResult List()
        {
           var table = from p in (new 擺腹BuyFoodContext()).TProducts
                    
                    select p;

            List<CProductViewModel> list = new List<CProductViewModel>();
            foreach (TProduct t in table)
            {
                list.Add(new CProductViewModel(t));
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult List(string pkeyword ,string pkeyword2)
        {
            IEnumerable<TProduct> table = null;
            string a = pkeyword2;
            if (string.IsNullOrEmpty(pkeyword))
            {
                table = from p in (new 擺腹BuyFoodContext()).TProducts
                        select p;
            }
            else
            {
                table = from p in (new 擺腹BuyFoodContext()).TProducts
                        where((p.CProductName).ToLower()).Contains(pkeyword.ToLower())

                        select p;
            }
            if(pkeyword2 != null)
            {
                table = from p in (new 擺腹BuyFoodContext()).TProducts
                         where p.CCategoryId ==  int.Parse(pkeyword2)
                         select p;
            }

            List<CProductViewModel> list = new List<CProductViewModel>();
            foreach (TProduct t in table)
            {
                list.Add(new CProductViewModel(t));
            }
            return View(list);
        }

        public JsonResult CheckAccount()
        {

            var data = from p in (new 擺腹BuyFoodContext()).TProductCategories
                       select p;
            // List<string> list = new List<string>();

            //foreach(var ppp in date)
            // {
            //     list.Add(ppp.CCategoryName);
            // }
            // string a = string.Join(',', list);
            // return a;
            return Json(data);
        }


        public IActionResult ListCategory()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = from pc in db.TProductCategories
                        select pc;
            List<CCategoryViewModel> list = new List<CCategoryViewModel>();
            foreach(TProductCategory item in table)
            {
                list.Add(new CCategoryViewModel (item));
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
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            if (!string.IsNullOrEmpty(p_產品類別新增.CCategoryName))
            {
                db.TProductCategories.Add(p_產品類別新增.allTProductCategory);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Create()
        {
            var data = from p in (new 擺腹BuyFoodContext()).TProductCategories   //假設從資料庫撈出資料。
                       select p;

            List<SelectListItem> mySelectItemList = new List<SelectListItem>();

            foreach (var item in data)
            {
                mySelectItemList.Add(new SelectListItem()
                {
                    Text = item.CCategoryName,
                    Value = item.CProductCategoryId.ToString(),
                    Selected = false
                });
            }

            var data1 = from p in (new 擺腹BuyFoodContext()).TEatPeriods   //假設從資料庫撈出資料。
                       select p;

            List<SelectListItem> mySelectItemList1 = new List<SelectListItem>();

            foreach (var item in data1)
            {
                mySelectItemList1.Add(new SelectListItem()
                {
                    Text = item.CEatPeriodName,
                    Value = item.CEatPeriodId.ToString(),
                    Selected = false
                });
            }
            var data2 = from p in (new 擺腹BuyFoodContext()).TIsOnSales   //假設從資料庫撈出資料。
                       select p;

            List<SelectListItem> mySelectItemList2 = new List<SelectListItem>();

            foreach (var item in data2)
            {
                mySelectItemList2.Add(new SelectListItem()
                {
                    Text = item.CStatusName,
                    Value = item.CIsOnSaleId.ToString(),
                    Selected = false
                });
            }

            CProductViewModel model = new CProductViewModel() //上面的 Model
            {
                CcategoryComBox = mySelectItemList,
                CEatTimeIdComBox=mySelectItemList1,
                CIsOnSaleIdComBox=mySelectItemList2
            };

            return View(model);

        }
        [HttpPost]
        public IActionResult Create(CProductViewModel p_產品新增)
        {
            string photoName = Guid.NewGuid().ToString()+".jpg";
            if (p_產品新增.Image != null)
            {
                using (var phtot = new FileStream(iv_host.ContentRootPath + @"\wwwroot\Images\" + photoName, FileMode.Create))
                {
                    p_產品新增.Image.CopyTo(phtot);
                }
            }
           
            p_產品新增.CPicture = "~/Images/" + photoName;
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            if (!string.IsNullOrEmpty(p_產品新增.CProductName))
            {
                db.TProducts.Add(p_產品新增.allProduct);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        //參數必須寫  id
        public IActionResult Delete(int? id)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            TProduct p_產品刪除 = db.TProducts.FirstOrDefault(p=>p.CProductId == id);

            if (id != null)
            {
                //刪除實體照片
                string p_產品照片移除 =Url.Content(p_產品刪除.CPicture);
            if (!string.IsNullOrEmpty(p_產品照片移除))
            {
                string p產品實體路徑 = iv_host.WebRootPath + p_產品照片移除;
                System.IO.File.Delete(p產品實體路徑);
            }
            
                db.TProducts.Remove(p_產品刪除);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public  IActionResult Edit(int? id)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var data = from p in (new 擺腹BuyFoodContext()).TProductCategories   //假設從資料庫撈出資料。
                       select p;

            List<SelectListItem> mySelectItemList = new List<SelectListItem>();

            foreach (var item in data)
            {
                mySelectItemList.Add(new SelectListItem()
                {
                    Text = item.CCategoryName,
                    Value = item.CProductCategoryId.ToString(),
                    Selected = false
                });
            }

            var data1 = from p in (new 擺腹BuyFoodContext()).TEatPeriods   //假設從資料庫撈出資料。
                        select p;

            List<SelectListItem> mySelectItemList1 = new List<SelectListItem>();

            foreach (var item in data1)
            {
                mySelectItemList1.Add(new SelectListItem()
                {
                    Text = item.CEatPeriodName,
                    Value = item.CEatPeriodId.ToString(),
                    Selected = false
                });
            }
            var data2 = from p in (new 擺腹BuyFoodContext()).TIsOnSales   //假設從資料庫撈出資料。
                        select p;

            List<SelectListItem> mySelectItemList2 = new List<SelectListItem>();

            foreach (var item in data2)
            {
                mySelectItemList2.Add(new SelectListItem()
                {
                    Text = item.CStatusName,
                    Value = item.CIsOnSaleId.ToString(),
                    Selected = false
                });
            }

            CProductViewModel model = new CProductViewModel() //上面的 Model
            {
                CcategoryComBox = mySelectItemList,
                CEatTimeIdComBox = mySelectItemList1,
                CIsOnSaleIdComBox = mySelectItemList2,
                //new CProductViewModel(p_產品修改)
            };

            if (id != null)
            {
                TProduct p_產品修改 = db.TProducts.FirstOrDefault(n => n.CProductId == id);
                if(id != null)
                {
                    return View(new CProductViewModel(p_產品修改));
                }
            }
            return RedirectToAction("List");

        }

        [HttpPost]
        public IActionResult Edit(CProductViewModel p_產品修改)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            TProduct l_product被修改 = db.TProducts.FirstOrDefault(n => n.CProductId == p_產品修改.CProductId);
            if(l_product被修改 != null)
            {

                string p_產品照片相對路徑 = Url.Content(l_product被修改.CPicture);
                if (!string.IsNullOrEmpty(p_產品照片相對路徑)&& p_產品修改.Image != null)
                {
                    string p產品實體路徑 = iv_host.WebRootPath + p_產品照片相對路徑;
                    System.IO.File.Delete(p產品實體路徑);
                }

                string photoName = Guid.NewGuid().ToString() + ".jpg";

                string p_照片最後路徑 = null;

                if (p_產品修改.Image != null)
                {
                    using (var phtot = new FileStream(iv_host.WebRootPath + @"\images\" + photoName, FileMode.Create))
                    {
                        p_產品修改.Image.CopyTo(phtot);
                        p_照片最後路徑 = "~/images/" + photoName;
                    }
                }
                else
                {
                    p_照片最後路徑 = p_產品照片相對路徑;
                }

                p_產品修改.CPicture = p_照片最後路徑;
                l_product被修改.CProductName = p_產品修改.CProductName;
                l_product被修改.CQuantity = p_產品修改.CQuantity;
                l_product被修改.CPrice = p_產品修改.CPrice;
                l_product被修改.CDescription = p_產品修改.CDescription;

                l_product被修改.CPicture = p_產品修改.CPicture;
                l_product被修改.CCategoryId = p_產品修改.CCategoryId;
                l_product被修改.CIsOnSaleId = p_產品修改.CIsOnSaleId;
                l_product被修改.CEatTimeId = p_產品修改.CEatTimeId;

                db.SaveChanges();
            }

            return RedirectToAction("List");
        }

    }
}
