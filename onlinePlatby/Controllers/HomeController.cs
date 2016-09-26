using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onlinePlatby.ActionFilters;
using onlinePlatby.Models;
using System.IO;

namespace onlinePlatby.Controllers
{
    [WhitespaceFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}