using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCexample.Models;
namespace MVCexample.Controllers
{
    public class ProductCategoryViewModels
    {
        public List<category> Momcategories { get; set; }
        //public List<categoryLowerTitle> LowerTitlecategories { get; set; }
        //public List<categoryLower> Lowercategories { get; set; }

        public List<Product> products { get; set; }
        public ProductCategoryViewModels(List<Product> products/*,List<category> Momcategories, List<categoryLowerTitle> LowerTitlecategories, List<categoryLower> Lowercategories*/)
        {
            this.products = products;
            //this.Momcategories = Momcategories;
            //this.LowerTitlecategories = LowerTitlecategories;
            //this.Lowercategories = Lowercategories;
        }
    }
}