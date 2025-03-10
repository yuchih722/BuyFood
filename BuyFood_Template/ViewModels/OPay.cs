﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class OPay
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string StoreID { get; set; }
        public int RtnCode { get; set; }
        public string RtnMsg { get; set; }
        public string TradeNo { get; set; }
        public int TradeAmt { get; set; }
        public int PayAmt { get; set; }
        public int RedeemAmt { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public int PaymentTypeChargeFee { get; set; }
        public string TradeDate { get; set; }
        public string ItemName { get; set; }
        public int SimulatePaid { get; set; }
        public int Gwsr { get; set; }
        public string process_date { get; set; }
        public string auth_code { get; set; }
        public int amount { get; set; }
        public int stage { get; set; }
        public int stast { get; set; }
        public int staed { get; set; }
        public int eci { get; set; }
        public string card4no { get; set; }
        public string card6no { get; set; }
        public int red_dan { get; set; }
        public int red_de_amt { get; set; }
        public int red_ok_amt { get; set; }
        public int red_yet { get; set; }
        public string PeriodType { get; set; }
        public int Frequency { get; set; }
        public int ExecTimes { get; set; }
        public int PeriodAmount { get; set; }
        public int TotalSuccessTimes { get; set; }
        public int TotalSuccessAmount { get; set; }
        public string CheckMacValue { get; set; }
    }
}
