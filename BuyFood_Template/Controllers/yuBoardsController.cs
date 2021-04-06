using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuBoardsController : Controller
    {
        public IActionResult BoardsALL()
        {
            return View();
        }
        public IActionResult BoardsBreakfast()
        {
            return View();
        }
        public IActionResult BoardsLunch()
        {
            return View();
        }
        public IActionResult BoardsDinner()
        {
            return View();
        }
    }
}
