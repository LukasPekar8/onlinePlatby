using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlinePlatby.Models.ViewModels
{
    public class OrderVM
    {
        public List<ProductVM> ProductVMs { get; set; }
        public decimal FinalPrice { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}