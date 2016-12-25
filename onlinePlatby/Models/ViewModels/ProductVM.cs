using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlinePlatby.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public string MainImageFilePath { get; set; }
    }
}