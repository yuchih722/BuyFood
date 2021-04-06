using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BuyFood_Template.Controllers
{
    public class ProductDetailController : Controller
    {

       

        public IActionResult ProductData(int? id)  //在商品頁面所選擇的產品顯示出來
        {

            //var product = db.TProducts.Where(x => x.CProductId == id).FirstOrDefault();

            var product = new 擺腹BuyFoodContext().TProducts.Where(x => x.CProductId == id).FirstOrDefault();
            return View(product);

            //int? A = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Count();
            //int? B = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Sum(n => n.CScores);
            //ViewBag.Bcount = A;

            //try
            //{
            //    ViewBag.Bagv = B / A;
            //}
            //catch
            //{
            //    ViewBag.Bagv = 0;
            //}

            //return View(product);
        } 
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public JsonResult Boardget(int id)
        {
            var table = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CProductId == id).Select(n => n);


            return Json(table);
        }
        public JsonResult startJson(int id)
        {
            //抓出當前CProductId的留言數量及平均值
            int? Count = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Count();
            int? Sum = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Sum(n => n.CScores);

            int average = Count <= 0 ? 0 : (int)(Sum / Count);

            var result = db.TOrderDetails.Select(n => new { bcount = Count, bavg = average });

            return Json(result.FirstOrDefault());
        }
        public JsonResult productBoards(int id, int star)   //抓出留言內容
        {

            if (star == 99)
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).OrderByDescending(n => n.CScores)
                    .Select(n => new
                    {
                        membersphoto = n.COrder.CMember.CPicture,
                        membername = n.COrder.CMember.CName,
                        memberproduct = n.CProduct.CProductName,
                        memberstar = n.CScores,
                        memberreview = n.CReview,
                    });
                return Json(table);
            }
            else if (star == 0)
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1&&n.CReview!=null ).OrderByDescending(n => n.CScores)
                .Select(n => new
                {
                  membersphoto = n.COrder.CMember.CPicture,
                  membername = n.COrder.CMember.CName,
                  memberproduct = n.CProduct.CProductName,
                  memberstar = n.CScores,
                 memberreview = n.CReview,
                });
                return Json(table);
            }
            else
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == star).OrderByDescending(n => n.CScores)
            .Select(n => new
            {
                membersphoto = n.COrder.CMember.CPicture,
                membername = n.COrder.CMember.CName,
                memberproduct = n.CProduct.CProductName,
                memberstar = n.CScores,
                memberreview = n.CReview,
            });
                return Json(table);
            }

          
        }
        public JsonResult productstar(int id)  //動態抓出留言數量
        {
            int? all = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Count();
            int? one = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 1).Count();
            int? two = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 2).Count();
            int? three = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 3).Count();
            int? four = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 4).Count();
            int? five = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 5).Count();
            int? hasreview = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Select(n => n.CReview).Count();

            var table = db.TOrderDetails
               .Select(n => new
               {
                   all = all,
                   one = one,
                   two = two,
                   three = three,
                   four = four,
                   five = five,
                   hasreview = hasreview,
               });
            return Json(table.FirstOrDefault());
        }
        public JsonResult smelltit(int id)  //動態抓出該品項詳細
        {
            var table = db.TProducts.Where(n => n.CProductId == id).FirstOrDefault();
            var tablea=db.TProducts.Where(n => n.CProductId == id).Select(n=>n.CCategory.CCategoryName).ToList();
            var x = db.TProducts.Select(n => new { table, tablea }).FirstOrDefault();
            return Json(x);
        }
        public JsonResult gethotproduct()  //熱門商品
        {
            var table = (from x in db.TOrderDetails
                        group x by x.CProductId into g
                        orderby g.Count() descending
                        select new
                        {
                            產品 = g.Key,
                            數量 = g.Count(),
                        }).Take(4);

            
            List<int> get_hot_productID = new List<int>();
            List<TProduct> hot_product_row = new List<TProduct>();
            foreach (var a in table)
            {
                get_hot_productID.Add(a.產品);
            }

            for (int i = 0; i < get_hot_productID.Count(); i++)
            {
                TProduct gg= db.TProducts.Where(n => n.CProductId == get_hot_productID[i]).Select(n => n).FirstOrDefault();
                hot_product_row.Add(gg);
            }

            return Json(hot_product_row);
        }
        public JsonResult getProductsByCategory(string id, string br, string lu, string di)
        {
            var data = (new 擺腹BuyFoodContext()).TProducts.Where(n => n.CCategoryId == int.Parse(id) && n.CIsOnSaleId != 3);
            return Json(data);
        }
    }

}
