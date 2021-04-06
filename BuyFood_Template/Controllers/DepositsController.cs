using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http;

namespace BuyFood_Template.Controllers
{

    public class viewmodelforpay
    {
        public string MerchantTradeNo { get; set; }
        public string StoreID { get; set; }
        public int RtnCode { get; set; } 
        public int TradeAmt { get; set; }
    }
    public class DepositsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void saveDepositResult(viewmodelforpay aaa)
        {
            if (aaa.RtnCode == 1)
            {
                擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
                TDeposit result = new TDeposit
                {
                    CMemberId = int.Parse(aaa.StoreID),
                    CDepositTime = DateTime.Now,
                    CDepositAmount = aaa.TradeAmt
                };
                dbcontext.TDeposits.Add(result);
                TMember changeTarget = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(aaa.StoreID));
                changeTarget.CDeposit += aaa.TradeAmt;
                dbcontext.SaveChanges();
            }
        }

        public JsonResult buildOrderDeposit(string id, string set)
        {
            int depositAmount= 0;
            switch (set)
            {
                case "1":
                    depositAmount = 500;
                    break;
                case "2":
                    depositAmount = 1000;
                    break;
                case "3":
                    depositAmount = 2000;
                    break;
                case "4":
                    depositAmount = 5000;
                    break;
                default:
                    depositAmount = 0;
                    break;                    
            }

            string MerchantTradeNo = "D"+DateTime.Now.ToString("yyyyMMddHHmmss");
            string MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string ItemName = "儲值"+depositAmount;
            string ReturnURL = "https://localhost:44398/Deposits/saveDepositResult";
            string ClientBackURL = "https://localhost:44398/";

            var buildOrder = deposit(id, MerchantTradeNo, MerchantTradeDate,depositAmount, ItemName, ReturnURL, ClientBackURL);
            return buildOrder;
        }
        public JsonResult deposit(
            string memberID,
            string MerchantTradeNo,
            string MerchantTradeDate,
            int TotalAmount,
            string ItemName,
            string ReturnURL,
            string ClientBackURL)
        {
            string rawString = $"HashKey=5294y06JbISpM5x9" +
    $"&ChoosePayment=Credit&ClientBackURL={ClientBackURL}" +
    $"&CreditInstallment=" +
    $"&EncryptType=1" +
    $"&InstallmentAmount=" +
    $"&ItemName={ItemName}" +
    $"&MerchantID=2000132" +
    $"&MerchantTradeDate={MerchantTradeDate}" +
    $"&MerchantTradeNo={MerchantTradeNo}" +
    $"&PaymentType=aio" +
    $"&Redeem=" +
    $"&ReturnURL={ReturnURL}" +
    $"&StoreID={memberID}" +
    $"&TotalAmount={TotalAmount}" +
    $"&TradeDesc=建立信用卡測試訂單" +
    $"&HashIV=v77hoKGq4kWxNNIS";

            string urlString = WebUtility.UrlEncode(rawString).ToLower();
            SHA256 sha256 = SHA256.Create();
            string sha256String = GetHash(sha256, urlString).ToUpper();

            var bulidOrder = new
            {
                MerchantID = "2000132",
                MerchantTradeNo = MerchantTradeNo,
                MerchantTradeDate = MerchantTradeDate ,
                PaymentType = "aio",
                TotalAmount = TotalAmount,
                TradeDesc = "建立信用卡測試訂單",
                ItemName = ItemName,
                ReturnURL = ReturnURL,
                ChoosePayment = "Credit",
                StoreID = memberID,
                ClientBackURL = ClientBackURL,
                CreditInstallment = "",
                InstallmentAmount = "",
                Redeem = "",
                EncryptType = "1",
                CheckMacValue = sha256String,
            };
                return Json(bulidOrder);
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public JsonResult getDeposits(string id)
        {
            var data = (new 擺腹BuyFoodContext()).TDeposits.Where(n => n.CMemberId == int.Parse(id))
                .Select(n => new
                {
                    depositTime = n.CDepositTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    depositAmount = n.CDepositAmount
                }) ;
            return Json(data);
        }
    }
}
