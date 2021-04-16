using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class ComboController : Controller
    {

        public JsonResult getCombo()
        {
            int memberID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            擺腹BuyFoodContext dbcontext = (new 擺腹BuyFoodContext());
            var datanew = dbcontext.TCombos.Where(n => n.CMemberId == memberID).Select(n => new
            {
                memberID=memberID,
                n.CComboId,
                n.CComboName,
                comboCount = n.TComboDetails.Count(),
                comboNotSalesCount = n.TComboDetails.Count(m => m.CProduct.CIsOnSaleId == 3),
                comboSum = n.TComboDetails.Sum(m => m.CProduct.CPrice),
                comboDetails = n.TComboDetails.Select(m => new
                {
                    m.CProductId,
                    m.CProduct,
                }),
                comboB = n.TComboDetails.Any(m=>m.CProduct.CIsBreakFast==0)? "0":"1",
                comboL = n.TComboDetails.Any(m=>m.CProduct.CIsLunch==0)? "0":"1",
                comboD=n.TComboDetails.Any(m=>m.CProduct.CIsDinner==0)? "0":"1"
            });

            return Json(datanew);
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
