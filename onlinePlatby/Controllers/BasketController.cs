using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlinePlatby.Controllers
{
    public class BasketController : Controller
    {
        // GET: Baske
        public ActionResult Index()
        {
            return View();
        }
    }
}