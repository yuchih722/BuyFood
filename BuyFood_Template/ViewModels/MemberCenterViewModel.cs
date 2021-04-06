using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class MemberCenterViewModel
    {
        private TMember iv_member = null;
        public MemberCenterViewModel(TMember p_member)
        {
            iv_member = p_member;
        }
        public MemberCenterViewModel()
        {
            iv_member = new TMember();
        }
        public vProduct Product { get { return new vProduct(); } }
        public vProdcutCategory ProdcutCategory { get { return new vProdcutCategory(); } }
        public vComboDetail ComboDetail { get { return new vComboDetail(); } }
        public vMember Member { get {return new vMember(); } }
        public vDeposits Deposits { get { return new vDeposits(); } }
        public vCombo Combos { get { return new vCombo(); } }
        public int CMemberId { 
            get { return iv_member.CMemberId; }
            set { iv_member.CMemberId = value ; } }
        public string CName
        {
            get { return iv_member.CName; }
            set { iv_member.CName = value; }
        }
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
        public string CGender
        {
            get { return iv_member.CGender; }
            set { iv_member.CGender = value; }
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
        public string CPhone
        {
            get { return iv_member.CPhone; }
            set { iv_member.CPhone = value; }
        }
        public string CPicture
        {
            get { return iv_member.CPicture; }
            set { iv_member.CPicture = value; }
        }
        public int? CFreezeCount
        {
            get { return iv_member.CFreezeCount; }
            set { iv_member.CFreezeCount = value; }
        }
        public string CReferrerCode
        {
            get { return iv_member.CReferrerCode; }
            set { iv_member.CReferrerCode = value; }
        }
        public int? CReferrerId
        {
            get { return iv_member.CReferrerId; }
            set { iv_member.CReferrerId = value; }
        }
        public TMember getMember { get { return iv_member; } }
    }
}
