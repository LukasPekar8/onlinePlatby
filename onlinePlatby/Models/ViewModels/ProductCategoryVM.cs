using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlinePlatby.Models.ViewModels
{
    public class ProductCategoryVM
    {
        public ProductCategory ProductCategory { get; set; }
        public List<ProductVM> Products { get; set; }
    }
}