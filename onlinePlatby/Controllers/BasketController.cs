using onlinePlatby.DbContexts;
using onlinePlatby.Models;
using onlinePlatby.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace onlinePlatby.Controllers
{
    public class BasketController : Controller
    {
        private onlinePlatbyEntities3 db = new onlinePlatbyEntities3();
        // Constructor
        public BasketController()
        {
            List<ProductCategory> productCategories = this.db.ProductCategories.ToList();
            ViewBag.ProductCategories = productCategories;
        }

        // GET: Baske
        public ActionResult Index()
        {
            int cookieId = -1;

            if (Request.Cookies["userId"] != null && (Request.Cookies["userId"].Value != ""))
                cookieId = int.Parse(Server.HtmlEncode(Request.Cookies["userId"].Value));

            Basket basket = new Basket();
            List<BasketProduct> basketProducts = new List<BasketProduct>();


            if (cookieId != -1)
            {
                if (this.db.Baskets.Any(b => b.Id == cookieId))
                {
                    basket = this.db.Baskets.Find(cookieId);
                    basketProducts = this.db.BasketProducts.Where(b => b.BasketId == basket.Id).ToList();
                }
            }

            if (basketProducts.Count() > 0)
            {
                List<ProductVM> productVMs = new List<ProductVM>();
                BasketVM vm = new BasketVM();
                vm.Basket = basket;

                foreach (BasketProduct b in basketProducts)
                {
                    ProductVariant productVariant = this.db.ProductVariants.Find(b.ProductVariantId);
                    Product product = this.db.Products.Find(productVariant.ProductId);
                    string mainFileFilePath = this.db.Images.Find(productVariant.MainImageId).Url;

                    ProductVM pVM = new ProductVM();
                    pVM.MainImageFilePath = mainFileFilePath;
                    pVM.Product = product;
                    pVM.ProductVariant = productVariant;

                    productVMs.Add(pVM);
                }

                vm.ProductVMs = productVMs;
                return View(vm);
            }
            return View(new BasketVM());
        }

        public string AddBasketProduct(int ProductVariantId)
        {
            try
            {
                ProductVariant productVariant = this.db.ProductVariants.Find(ProductVariantId);
                Basket basket = null;

                int cookieId = -1;

                if (Request.Cookies["userId"] != null && (Request.Cookies["userId"].Value != ""))
                    cookieId = int.Parse(Server.HtmlEncode(Request.Cookies["userId"].Value));
                else
                {
                    // Creation of the basket
                    basket = new Basket();
                    basket.IPAddress = "k";
                    basket.LastDateEdited = DateTime.Now;
                    basket.CreatedAt = DateTime.Now;

                    this.db.Baskets.Add(basket);
                    this.db.SaveChanges();
                    basket = this.db.Baskets.Find(this.db.Baskets.Max(b => b.Id));

                    HttpCookie userIdCookie = new HttpCookie("userId");
                    userIdCookie.Value = basket.Id.ToString();

                    Response.Cookies.Add(userIdCookie);
                    cookieId = basket.Id;
                }

                if (cookieId != -1)
                {
                    basket = this.db.Baskets.Find(cookieId);
                }

                // Chech if the product already exist
                bool isTaken = false;
                foreach (var item in this.db.BasketProducts.Where(b => b.BasketId == basket.Id))
                {
                    if (item.ProductVariantId == ProductVariantId)
                    {
                        isTaken = true;
                    }
                }
                if (!isTaken)
                {
                    // Now I have basket and product variant
                    this.db.BasketProducts.Add(new BasketProduct { BasketId = basket.Id, ProductVariantId = productVariant.Id });
                    this.db.SaveChanges();

                    return "success";
                }
                else
                {
                    return "Produkt již máte v košíku";
                }
            }
            catch(Exception e)
            {
                return "false";
            }
        }

        /// <summary>
        /// Deletes the product from the basket
        /// </summary>
        /// <param name="BasketId"></param>
        /// <param name="ProductVariantId"></param>
        /// <returns></returns>
        public ActionResult DeleteBasketProduct(int? BasketId, int? ProductVariantId)
        {
            ProductVariant productVariant = this.db.ProductVariants.Find(ProductVariantId);

            BasketProduct bProduct = this.db.BasketProducts.Where(b => b.ProductVariantId == ProductVariantId).FirstOrDefault();

            this.db.BasketProducts.Remove(bProduct);
            this.db.SaveChanges();

            return RedirectToAction("Index", "Basket");
        }

        [HttpPost]
        public ActionResult MakeOrder(string payment, string transportation, int[] productVariants, int[] counts, string email, string name, string phone)
        {
            List<ProductVM> productVMs = new List<ProductVM>();
            decimal finalPrice = 0;

            for (int i = 0; i < productVariants.Count(); i++)
            {
                ProductVariant productVariant = this.db.ProductVariants.Find(productVariants[i]);
                Product product = this.db.Products.Find(productVariant.ProductId);
                string mainImageFilePath = this.db.Images.Find(productVariant.MainImageId).Url;
                productVMs.Add(new ProductVM { ProductVariant = productVariant, Product = product, MainImageFilePath = mainImageFilePath });

                decimal thisProductPrice = productVariant.SalePrice * counts[i];
                finalPrice += thisProductPrice;
            }

            if (payment == "paypal") { 
                TempData["ProductVMs"] = productVMs;
                return RedirectToAction("PayPal", "Payment", new { FinalPrice = finalPrice, Email = email, Name = name, Phone = phone });
            }

            if (payment == "stripe")
            {
                TempData["ProductVMs"] = productVMs;
                return RedirectToAction("Stripe", "Payment", new { FinalPrice = finalPrice, Email = email, Name = name, Phone = phone });
            }

            return RedirectToAction("Index");
        }

        public string OrderSuccess( string email, string name, decimal finalPrice)
        {
            try
            {
                // Sending the mail
                MailAddress from = new MailAddress("bonsaiwebform@gmail.com", "Bonsai-Development - Formulář");
                MailAddress to = new MailAddress(email);
                EmailManager emailManager = new EmailManager(from, "H0d0r-123456");
                string body = "Objednávka přijata <br /> Jméno: " + name + "<br /> Email: " + email + "<br /> Cena objednávky: " + finalPrice.ToString() + " Kč <br /> Děkujeme za nákup.";
                emailManager.SendEmail(to, "Objednávka přijata", body);

                // Deleting the basket (if exists)
                int cookieId = -1;
                if (Request.Cookies["userId"] != null)
                {
                    cookieId = int.Parse(Server.HtmlEncode(Request.Cookies["userId"].Value));
                    HttpCookie myCookie = Request.Cookies["userId"];
                    myCookie.Value = null;
                    Response.Cookies.Add(myCookie);
                }

                if (cookieId != -1)
                {
                List<BasketProduct> basketProducts = this.db.BasketProducts.Where(b => b.BasketId == cookieId).ToList();
                foreach (BasketProduct b in basketProducts)
                {
                    this.db.BasketProducts.Remove(b);
                }
                this.db.Baskets.Remove(this.db.Baskets.Find(cookieId));
                this.db.SaveChanges();
                }

                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        [HttpPost]
        public ActionResult StripeSuccess(string email ,string name, decimal finalPrice)
        {
                // Sending the mail
                MailAddress from = new MailAddress("bonsaiwebform@gmail.com", "Bonsai-Development - Formulář");
                MailAddress to = new MailAddress(email);
                EmailManager emailManager = new EmailManager(from, "H0d0r-123456");
                string body = "Objednávka přijata <br /> Jméno: " + name + "<br /> Email: " + email + "<br /> Cena objednávky: " + finalPrice.ToString() + " Kč <br /> Děkujeme za nákup.";
                emailManager.SendEmail(to, "Objednávka přijata", body);

                // Deleting the basket (if exists)
                int cookieId = -1;
                if (Request.Cookies["userId"] != null)
                {
                    cookieId = int.Parse(Server.HtmlEncode(Request.Cookies["userId"].Value));
                    HttpCookie myCookie = Request.Cookies["userId"];
                    myCookie.Value = null;
                    Response.Cookies.Add(myCookie);
                }

                if (cookieId != -1)
                {
                    List<BasketProduct> basketProducts = this.db.BasketProducts.Where(b => b.BasketId == cookieId).ToList();
                    foreach (BasketProduct b in basketProducts)
                    {
                        this.db.BasketProducts.Remove(b);
                    }
                    this.db.Baskets.Remove(this.db.Baskets.Find(cookieId));
                    this.db.SaveChanges();
                }

            return RedirectToAction("OrderIsSuccessful");
        }

        public ActionResult OrderIsSuccessful()
        {
            return View();
        }
    }
}