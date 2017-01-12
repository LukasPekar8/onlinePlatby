using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlinePlatby.Models.ViewModels
{
    public class BasketVM
    {
        public Basket Basket { get; set; }
        public List<ProductVM> ProductVMs { get; set; }
    }
}