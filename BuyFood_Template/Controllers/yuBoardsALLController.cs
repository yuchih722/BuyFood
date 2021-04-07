using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuBoardsALLController : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult BoardsALL()
        {
            ViewBag.All = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1).Select(n => n).Count();
            ViewBag.five = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1&&n.CScores==5).Select(n => n).Count();
            ViewBag.four = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CScores == 4).Select(n => n).Count();
            ViewBag.three = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CScores == 3).Select(n => n).Count();
            ViewBag.two = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CScores == 2).Select(n => n).Count();
            ViewBag.one = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CScores == 1).Select(n => n).Count();
            return View();
        }

        #region 全部留言 內容
        public JsonResult BoardsJSONList(int? num)
        {
            if (num != null)
            {
                if (num == 99)
                {
                  var   od = from x in db.TOrderDetails
                         where x.CFeedBackStatus == 1
                         orderby x.COrderId descending
                         select new
                         {
                             COrderDetailId=x.COrderDetailId,
                             COrderId = x.COrderId,
                             CMemberId= x.COrder.CMemberId,
                             CCategoryName=x.CProduct.CCategory.CCategoryName,
                             CProductName=x.CProduct.CProductName,
                             CReview=x.CReview,
                             CScores=x.CScores,
                         };
                    return Json(od.ToList());
                }
                else
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1&&x.CScores==num
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,

                             };
                    return Json(od.ToList());
                }

            }
            
            return Json("") ;
          
        }
        #endregion

        #region 早餐留言 內容
        public JsonResult BoardsJSONListbreakfast(int? num)
        {
            if (num != null)
            {
                if (num == 99)
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1&&x.CProduct.CIsBreakFast==1
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderDetailId = x.COrderDetailId,
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,
                             };
                    return Json(od.ToList());
                }
                else
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1 && x.CProduct.CIsBreakFast == 1 && x.CScores == num
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,

                             };
                    return Json(od.ToList());
                }

            }

            return Json("");

        }
        #endregion

        #region 午餐留言 內容
        public JsonResult BoardsJSONListlunch(int? num)
        {
            if (num != null)
            {
                if (num == 99)
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1 && x.CProduct.CIsLunch == 1
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderDetailId = x.COrderDetailId,
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,
                             };
                    return Json(od.ToList());
                }
                else
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1 && x.CProduct.CIsLunch == 1 && x.CScores == num
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,

                             };
                    return Json(od.ToList());
                }

            }

            return Json("");

        }
        #endregion

        #region 晚餐留言 內容
        public JsonResult BoardsJSONListdinner(int? num)
        {
            if (num != null)
            {
                if (num == 99)
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1 && x.CProduct.CIsDinner == 1
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderDetailId = x.COrderDetailId,
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,
                             };
                    return Json(od.ToList());
                }
                else
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 1 && x.CProduct.CIsDinner == 1 && x.CScores == num
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,

                             };
                    return Json(od.ToList());
                }

            }

            return Json("");

        }
        #endregion

        #region 黑名單留言 內容
        public JsonResult BoardsJSONListblack(int? num)
        {
            if (num != null)
            {
                if (num == 99)
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 0 
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderDetailId = x.COrderDetailId,
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,
                             };
                    return Json(od.ToList());
                }
                else
                {
                    var od = from x in db.TOrderDetails
                             where x.CFeedBackStatus == 0&& x.CScores == num
                             orderby x.COrderId descending
                             select new
                             {
                                 COrderId = x.COrderId,
                                 CMemberId = x.COrder.CMemberId,
                                 CCategoryName = x.CProduct.CCategory.CCategoryName,
                                 CProductName = x.CProduct.CProductName,
                                 CReview = x.CReview,
                                 CScores = x.CScores,

                             };
                    return Json(od.ToList());
                }

            }

            return Json("");

        }
        #endregion
 

        public IActionResult BoardsBreakfast()
        {
            ViewBag.All = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast==1&& n.CFeedBackStatus == 1).Select(n => n).Count();
            ViewBag.five = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast == 1 && n.CFeedBackStatus == 1 && n.CScores == 5 ).Select(n => n).Count();
            ViewBag.four = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast == 1 && n.CFeedBackStatus == 1 && n.CScores == 4 ).Select(n => n).Count();
            ViewBag.three = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast == 1 && n.CFeedBackStatus == 1 && n.CScores == 3 ).Select(n => n).Count();
            ViewBag.two = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast == 1 && n.CFeedBackStatus == 1 && n.CScores == 2 ).Select(n => n).Count();
            ViewBag.one = db.TOrderDetails.Where(n => n.CProduct.CIsBreakFast == 1 && n.CFeedBackStatus == 1 && n.CScores == 1 ).Select(n => n).Count();

            return View();
        }
        public IActionResult BoardsLunch()
        {
            ViewBag.All = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1).Select(n => n).Count();
            ViewBag.five = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1 && n.CScores == 5).Select(n => n).Count();
            ViewBag.four = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1 && n.CScores == 4).Select(n => n).Count();
            ViewBag.three = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1 && n.CScores == 3).Select(n => n).Count();
            ViewBag.two = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1 && n.CScores == 2).Select(n => n).Count();
            ViewBag.one = db.TOrderDetails.Where(n => n.CProduct.CIsLunch == 1 && n.CFeedBackStatus == 1 && n.CScores == 1).Select(n => n).Count();
            return View();
        }
        public IActionResult BoardsDinner()
        {
            ViewBag.All = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1).Select(n => n).Count();
            ViewBag.five = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1 && n.CScores == 5).Select(n => n).Count();
            ViewBag.four = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1 && n.CScores == 4).Select(n => n).Count();
            ViewBag.three = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1 && n.CScores == 3).Select(n => n).Count();
            ViewBag.two = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1 && n.CScores == 2).Select(n => n).Count();
            ViewBag.one = db.TOrderDetails.Where(n => n.CProduct.CIsDinner == 1 && n.CFeedBackStatus == 1 && n.CScores == 1).Select(n => n).Count();
            return View();
        }
        public IActionResult Boardsblacklist()
        {
            ViewBag.All = db.TOrderDetails.Where(n => n.CFeedBackStatus == 0).Select(n => n).Count();
            return View();
        }

        public void gotoblack(int? id)
        {
            TOrderDetail table = db.TOrderDetails.FirstOrDefault(n => n.COrderDetailId == id);

            if (table != null)
            {
                if (table.CFeedBackStatus == 1)
                    table.CFeedBackStatus = 0;
                else
                    table.CFeedBackStatus = 1;
            }
            db.SaveChanges();
        }
        public void DetailBoardDelete([FromBody] List<yupushstrViewModel> Array)
        {
            if (Array != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                for (int i = 0; i < Array.Count; i++)
                {
                    TOrderDetail table = db.TOrderDetails.FirstOrDefault(a => a.COrderDetailId.ToString() == Array[i].strmember);
                    if (table != null)
                    {
                        if (table.CFeedBackStatus == 1)
                            table.CFeedBackStatus = 0;
                        else
                            table.CFeedBackStatus = 1;
                        db.Remove(table);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
