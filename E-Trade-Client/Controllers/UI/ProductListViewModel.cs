using MVCexample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCexample.Controllers
{
    public class ProductListViewModel
    {
        public List<Product> pList { get; set; }
        public List<cClass> cList { get; set; }
        public ProductListViewModel(List<Product> pList, List<cClass> cList)
        {
            this.pList = pList;
            this.cList = cList;
        }
    }
}