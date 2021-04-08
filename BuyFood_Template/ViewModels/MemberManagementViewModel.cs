using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http; //圖片
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModel
{
    public class MemberManagementViewModel
    {        
        private TDeposit iv_deposit = null;
        public TDeposit deposit {get { return iv_deposit;}}

        private TMember iv_member = null;
        public TMember member { get { return iv_member; } } //唯讀
        public MemberManagementViewModel(TMember p)
        {
            iv_member = p;
        }
        public MemberManagementViewModel()
        {
            iv_member = new TMember();
        }

        public int CMemberId
        {
            get { return iv_member.CMemberId; }
            set { iv_member.CMemberId = value; }
        }
        
        [DisplayName("會員姓名")]
        [Required(ErrorMessage = "姓名不可空白")]
        public string CName
        {
            get { return iv_member.CName; }
            set { iv_member.CName = value; }
        }
        [DisplayName("Email帳號")]
        [Required(ErrorMessage = "Email帳號不可空白")]
        [EmailAddress(ErrorMessage ="Email格式有誤")]
        public string CEmail
        {
            get { return iv_member.CEmail; }
            set { iv_member.CEmail = value; }
        }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "密碼不可空白")]
        public string CPassword
        {
            get { return iv_member.CPassword; }
            set { iv_member.CPassword = value; }
        }
        [DisplayName("姓別")]
        public string CGender
        {
            get { return iv_member.CGender; }
            set { iv_member.CGender = value; }
        }
        [DisplayName("電話")]
        [Required(ErrorMessage ="電話不可空白")]
        [StringLength(10,ErrorMessage ="電話必須是10位數",MinimumLength =10)]
        public string CPhone
        {
            get { return iv_member.CPhone; }
            set { iv_member.CPhone = value; }
        }
        [DisplayName("地址")]
        public string CAddress
        {
            get { return iv_member.CAddress; }
            set { iv_member.CAddress = value; }
        }
        [DisplayName("黑名單")]
        public int? CBlackList
        {
            get { return iv_member.CBlackList; }
            set { iv_member.CBlackList = value; }
        }
        [DisplayName("儲值金額")]
        public decimal? CDeposit
        {
            get { return iv_member.CDeposit; }
            set { iv_member.CDeposit = value; }
        }
        [DisplayName("會員照片")]
        public string CPicture
        {
            get { return iv_member.CPicture; }
            set { iv_member.CPicture = value; }
        }
        public IFormFile image { get; set; } //圖片

        //public decimal? CDepositAmount
        //{
        //    get { return iv_deposit.CDepositAmount; }
        //    set { iv_deposit.CDepositAmount = value; }
        //}
    }
}
