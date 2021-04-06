using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class MemberManagementViewModel
    {
        private TMember iv_member = null;
        private TDeposit iv_deposit = null;
        public TDeposit deposit { get { return iv_deposit; } }
        public TMember member { get { return iv_member; } }
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
        [Required(ErrorMessage = "姓名是必填欄位")]
        [DisplayName("會員姓名")]
        public string CName
        {
            get { return iv_member.CName; }
            set { iv_member.CName = value; }
        }
        [DisplayName("Email帳號")]
        public string CEmail
        {
            get { return iv_member.CEmail; }
            set { iv_member.CEmail = value; }
        }
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
        public string CPhone
        {
            get { return iv_member.CPhone; }
            set { iv_member.CPhone = value; }
        }
        public string CAddress
        {
            get { return iv_member.CAddress; }
            set { iv_member.CAddress = value; }
        }
        public int? CBlackList
        {
            get { return iv_member.CBlackList; }
            set { iv_member.CBlackList = value; }
        }
        public decimal? CDeposit
        {
            get { return iv_member.CDeposit; }
            set { iv_member.CDeposit = value; }
        }

        public decimal CDepositAmount
        {
            get { return iv_deposit.CDepositAmount; }
            set { iv_deposit.CDepositAmount = value; }
        }
    }
}
