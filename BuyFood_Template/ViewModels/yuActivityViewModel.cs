using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class yuActivityViewModel
    {
        private TActivity iv_Activity = null;
        public TActivity Activity { get { return iv_Activity; } }

        public yuActivityViewModel(TActivity p)
        {
            iv_Activity = p;
        }
        public yuActivityViewModel()
        {
            iv_Activity = new TActivity();
        }
        [DisplayName("活動編號")]
        public int CActivityId { get { return iv_Activity.CActivityId; } set { iv_Activity.CActivityId=value; } }
        [DisplayName("活動名稱")]
        public string CActivityName { get { return iv_Activity.CActivityName; } set {iv_Activity.CActivityName=value; } }
        [DisplayName("活動內容")]
        public string CDescription { get { return iv_Activity.CDescription; } set { iv_Activity.CDescription = value; } }
        [DisplayName("活動相片")]
        public string CPicture { get { return iv_Activity.CPicture; } set { iv_Activity.CPicture = value; } }
        [DisplayName("活動連結")]
        public string CLink { get { return iv_Activity.CLink; } set { iv_Activity.CLink = value; } }
        [DisplayName("輪播順序")]
        public int? CRank { get { return iv_Activity.CRank; } set { iv_Activity.CRank = value; } }
        [DisplayName("活動狀態")]
        public int? CStatus { get { return iv_Activity.CStatus; } set { iv_Activity.CStatus = value; } }
        [DisplayName("活動開始時間")]
        public string CTime { get { return iv_Activity.CTime; } set { iv_Activity.CTime = value; } }
        public IFormFile img { get; set; }

    }
}
