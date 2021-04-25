using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCexample.Models
{
    public partial class Product
    {
        public List<ProductDetail> pDetails { get; set; }
    }
}