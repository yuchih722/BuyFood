using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class cCustomerAnalysisController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
