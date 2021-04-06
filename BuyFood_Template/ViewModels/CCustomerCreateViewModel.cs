using Microsoft.AspNetCore.Http;
using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModel
{
    public class CCustomerCreateViewModel
    {
        private TMember iv_CreateMember = null;

        public TMember member { get { return iv_CreateMember; } }

        public CCustomerCreateViewModel(TMember CreateMember)
        {
            iv_CreateMember = CreateMember;
        }
        public CCustomerCreateViewModel()
        {
            iv_CreateMember = new TMember();
        }

        public int CMemberID
        {
            get { return iv_CreateMember.CMemberId; }
            set { iv_CreateMember.CMemberId = value; }
        }

        [DisplayName("姓名")]
        public string CName { get { return iv_CreateMember.CName; } set { iv_CreateMember.CName = value; } }
        [DisplayName("信箱")]
        public string CEmail { get { return iv_CreateMember.CEmail; } set { iv_CreateMember.CEmail = value; } }
        [DisplayName("密碼")]
        public string CPassword { get { return iv_CreateMember.CPassword; } set { iv_CreateMember.CPassword = value; } }
        [DisplayName("性別")]
        public string CGender { get { return iv_CreateMember.CGender; } set { iv_CreateMember.CGender = value; } }
        [DisplayName("地址")]
        public string CAddress { get { return iv_CreateMember.CAddress; } set { iv_CreateMember.CAddress = value; } }
        public int? CBlackList { get { return iv_CreateMember.CBlackList; } set { iv_CreateMember.CBlackList = value; } }
        public decimal? CDeposit { get { return iv_CreateMember.CDeposit; } set { iv_CreateMember.CDeposit = value; } }
        [DisplayName("手機")]
        public string CPhone { get { return iv_CreateMember.CPhone; } set { iv_CreateMember.CPhone = value; } }
        [DisplayName("上傳頭像")]
        public string CPicture { get { return iv_CreateMember.CPicture; } set { iv_CreateMember.CPicture = value; } }
        public IFormFile img { get; set; }
        public int? CFreezeCount { get { return iv_CreateMember.CFreezeCount; } set { iv_CreateMember.CFreezeCount = value; } }
    }
}
