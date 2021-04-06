using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModel
{
    public class CLoginViewModel
    {
        private TMember iv_Member = null;

        public TMember member { get { return iv_Member; } }

        public CLoginViewModel(TMember loginMember)
        {
            iv_Member = loginMember;
        }
        public CLoginViewModel()
        {
            iv_Member = new TMember();
        }

        [Required(ErrorMessage = "請輸入註冊信箱")]
        [DisplayName("註冊信箱")]
        public string CEmail { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼")]
        public string CPassword { get; set; }

        [Required(ErrorMessage = "請輸入認證碼")]
        [DisplayName("認證碼")]
        public string txtCode { get; set; }

    }
}
