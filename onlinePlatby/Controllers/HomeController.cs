using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onlinePlatby.ActionFilters;
using onlinePlatby.Models;
using System.IO;
using onlinePlatby.Models.ViewModels;

namespace onlinePlatby.Controllers
{
    [WhitespaceFilter]
    public class HomeController : Controller
    {
        private onlinePlatbyEntities3 db = new onlinePlatbyEntities3();
        // Constructor
        public HomeController()
        {
            List<ProductCategory> productCategories = this.db.ProductCategories.ToList();
            ViewBag.ProductCategories = productCategories;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int Id)
        {
            ProductCategoryVM vm = new ProductCategoryVM();

            // Getting product category
            ProductCategory productCategory = this.db.ProductCategories.Find(Id);
            if (productCategory == null)
            {
                throw new HttpException(404, "Kategorii se nepodařilo najít");
            }

            // Getting category products
            List<ProductVM> categoryProductsVM = new List<ProductVM>();
            List<CategoryProduct> catProductRow = this.db.CategoryProducts.Where(c => c.ProductCategoryId == productCategory.Id).ToList();
            foreach(CategoryProduct catProduct in catProductRow)
            {
                ProductVariant pVariant = this.db.ProductVariants.Find(catProduct.ProductVariantId);

                if (pVariant != null)
                {
                    // getting main image
                    Image image = this.db.Images.Find(pVariant.MainImageId);
                    string imagePath = image.Url;

                    Product product = this.db.Products.Find(pVariant.ProductId);
                    if (product != null)
                    {
                        ProductVM pVm = new ProductVM { Product = product, ProductVariant = pVariant, MainImageFilePath = imagePath };

                        categoryProductsVM.Add(pVm);
                    }
                }
            }

            // filling view model with data
            vm.ProductCategory = productCategory;
            vm.Products = categoryProductsVM;

            return View(vm);
        }

        public ActionResult ProductDetail(int Id)
        {
            ProductVM vm = new ProductVM();

            // Getting product variant
            vm.ProductVariant = this.db.ProductVariants.Find(Id);

            if (vm.ProductVariant != null) // is product variant there?
            {
                // Getting product
                vm.Product = this.db.Products.Find(vm.ProductVariant.ProductId); 
                if (vm.Product != null) // is there product to the variant?
                {
                    // Getting main Image
                    Image mainImage = this.db.Images.Find(vm.ProductVariant.MainImageId);
                    vm.MainImageFilePath = mainImage.Url;

                    return View(vm);
                }
            }

            throw new HttpException(404, "Product se nepodařilo najít");
        }
    }
}