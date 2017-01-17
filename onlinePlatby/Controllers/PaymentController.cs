using onlinePlatby.Models;
using onlinePlatby.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlinePlatby.Controllers
{
    public class PaymentController : Controller
    {
        private onlinePlatbyEntities3 db = new onlinePlatbyEntities3();
        // Constructor
        public PaymentController()
        {
            List<ProductCategory> productCategories = this.db.ProductCategories.ToList();
            ViewBag.ProductCategories = productCategories;
        }
        // GET: Payment
        public ActionResult PayPal(decimal FinalPrice, string Email, string Name, string Phone)
        {
            List<ProductVM> productVMs = TempData["ProductVMs"] as List<ProductVM>;

            OrderVM vm = new OrderVM{
                ProductVMs = productVMs,
                FinalPrice = FinalPrice,
                Name = Name,
                Email = Email, 
                Phone = Phone
            };
            return View(vm);
        }
    }
}