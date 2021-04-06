using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class ComboController : Controller
    {

        public JsonResult getCombo(string id)
        {
            擺腹BuyFoodContext dbcontext = (new 擺腹BuyFoodContext());

            var data = dbcontext.TComboDetails.
                Where(n => n.CCombo.CMemberId == int.Parse(id)).
                Select(n => new
                {
                    comboID = n.CCombo.CComboId,
                    comboName = n.CCombo.CComboName,
                    productID = n.CProduct.CProductId,
                    price = n.CProduct.CPrice,
                    productName = n.CProduct.CProductName,
                    n.CProduct.CIsOnSaleId
                });

            var data3 = data.
                GroupBy(n => n.comboID).
                Select(n => new
                {
                    ComboID = n.Key,
                    ComboName = data.FirstOrDefault(a => a.comboID == n.Key).comboName,
                    TotalPrice = n.Sum(p => p.price),
                    TotalItems = n.Count(),
                    NotSaleItem = n.Count(n => n.CIsOnSaleId == 3),
                    comboDetail = new List<sTComboDetail>()
                }).ToList();

            foreach (var item in data3)
            {
                List<sTComboDetail> Details = data.Where(n => n.comboID == item.ComboID)
                    .GroupBy(n => n.productID).Select(n => new sTComboDetail
                    {
                        productID = n.Key,
                        productName = data.FirstOrDefault(p => p.productID == n.Key).productName,
                        price = data.FirstOrDefault(p => p.productID == n.Key).price,
                        qty = n.Count(),
                        onSale = data.FirstOrDefault(p => p.productID == n.Key).CIsOnSaleId
                    }).ToList();

                foreach (var Detail in Details)
                {
                    item.comboDetail.Add(Detail);
                }
            }


            return Json(data3);
        }

        public JsonResult getComboDetail(string id)
        {

            var data = (new 擺腹BuyFoodContext()).TComboDetails
                .Where(n => n.CComboId == int.Parse(id))
                .Select(n => new
                {
                    n.CComboId,
                    n.CCombo.CComboName,
                    n.CProductId,
                    n.CProduct.CCategoryId,
                    n.CProduct.CProductName,
                    n.CProduct.CPrice,
                    n.CProduct.CIsBreakFast,
                    n.CProduct.CIsLunch,
                    n.CProduct.CIsDinner,
                    n.CProduct.CIsOnSaleId
                }).OrderByDescending(n => n.CProductId).ThenByDescending(n => n.CCategoryId);

            var data2 = data.GroupBy(n => n.CProductId)
                .Select(n => new
                {
                    productID = n.Key,
                    productName = data.FirstOrDefault(a => a.CProductId == n.Key).CProductName,
                    quantity = n.Count(),
                    unitPrice = data.FirstOrDefault(a => a.CProductId == n.Key).CPrice,
                    categoryID = data.FirstOrDefault(a => a.CProductId == n.Key).CCategoryId,
                    productOn = data.FirstOrDefault(a => a.CProductId == n.Key).CIsOnSaleId,
                    br = data.FirstOrDefault(a => a.CProductId == n.Key).CIsBreakFast,
                    lu = data.FirstOrDefault(a => a.CProductId == n.Key).CIsLunch,
                    di = data.FirstOrDefault(a => a.CProductId == n.Key).CIsDinner,
                });

            return Json(data2);

        }


        public void saveCombo([FromBody] List<jComboDetail> list)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            if (list.First().cID != 0)
            {
                TCombo reviseTarget = dbcontext.TCombos.FirstOrDefault(n => n.CComboId == list.First().cID);
                reviseTarget.CComboName = list.First().cName;
                var removetarget = dbcontext.TComboDetails.Where(n => n.CComboId == list[0].cID);
                dbcontext.TComboDetails.RemoveRange(removetarget);

                foreach (var item in list)
                {
                    for (int i = 0; i < item.q; i++)
                    {
                        TComboDetail detail = new TComboDetail
                        {
                            CComboId = item.cID,
                            CProductId = item.pID,
                        };
                        dbcontext.TComboDetails.Add(detail);
                    };
                };
                dbcontext.SaveChanges();
            }
            else
            {
                TCombo newcombo = new TCombo
                {
                    CComboName = list.First().cName,
                    CMemberId = list.First().mID,
                };

                dbcontext.TCombos.Add(newcombo);
                dbcontext.SaveChanges();

                int newcomboID = dbcontext.TCombos.Where(n => n.CMemberId == list.First().mID).OrderByDescending(n => n.CComboId).First().CComboId;
                foreach (var item in list)
                {
                    for (int i = 0; i < item.q; i++)
                    {
                        TComboDetail detail = new TComboDetail
                        {
                            CComboId = newcomboID,
                            CProductId = item.pID,
                        };
                        dbcontext.TComboDetails.Add(detail);
                    };
                };
                dbcontext.SaveChanges();
            }
            //return "done";
        }

        //public JsonResult addCombo(string id)
        //{
        //    擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
        //    var combos = dbcontext.TCombos.Where(n => n.CMemberId == int.Parse(id)).Count() +1;
        //    TCombo newCombo = new TCombo
        //    {
        //        CComboName = "套餐" + combos,
        //        CMemberId = int.Parse(id)
        //    };
        //    dbcontext.TCombos.Add(newCombo);
        //    dbcontext.SaveChanges();
        //    TCombo data = dbcontext.TCombos.Where(n => n.CMemberId == int.Parse(id)).OrderByDescending(n => n.CComboId).FirstOrDefault();

        //    return Json(data);
        //}

        public void deleteCombo(string id)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            var comboDetails = dbcontext.TComboDetails.Where(n => n.CComboId == int.Parse(id));
            dbcontext.TComboDetails.RemoveRange(comboDetails);

            var combo = dbcontext.TCombos.FirstOrDefault(n => n.CComboId == int.Parse(id));
            dbcontext.TCombos.Remove(combo);
            dbcontext.SaveChanges();
        }
    }

}
