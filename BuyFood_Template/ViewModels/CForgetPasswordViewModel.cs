using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CForgetPasswordViewModel
    {
        private TMember iv_Member = null;

        public TMember member { get { return iv_Member; } }

        public CForgetPasswordViewModel(TMember loginMemberForgetPwd)
        {
            iv_Member = loginMemberForgetPwd;
        }
        public CForgetPasswordViewModel()
        {
            iv_Member = new TMember();
        }

        public string CEmail { get; set; }

        public string CPhone { get; set; }

    }
}
