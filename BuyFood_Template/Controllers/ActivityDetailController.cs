using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class ActivityDetailController : Controller
    {
        public IActionResult ShowActivityDetail()
        {
            return View();
        }
    }
}
