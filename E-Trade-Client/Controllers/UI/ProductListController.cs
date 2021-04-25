using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCexample.Models;
using System.Collections;
using System.Web.Script.Serialization;
using MVCexample.Helper;
using MVCexample.Controllers.API;

namespace MVCexample.Controllers
{

    public class cClass
    {
        public string name { get; set; }
        public int count { get; set; }
    }

    [AllowAnonymous]
    public class ProductListController : BaseController
    {
        ServisController api = new ServisController();
        static List<Product> list;
        static int _ID = 0;
        public ActionResult Index(int? ID, string BrandName)
        {
            _ID = ID ?? _ID;
            list = api.getProductByCategoryLowerID(_ID);

            List<cClass> cList = list.OrderBy(i => i.ID)
                            .GroupBy(i => i.Brand)
                            .Select(i => new cClass { name = i.Key, count = i.Count() }).ToList();

            int cLowerTitleID = (int)list[0].CategoryLowerTitleID;
            int cID = (int)list[0].CategoryID;
            string cName = api.getcategorys().Where(i => i.ID == cID).Select(i => i.Name).FirstOrDefault();
            string cLowerTitleName = api.getcategoryLowerTitles().Where(i => i.ID == cLowerTitleID).Select(i => i.Name).FirstOrDefault();
            string cLowerame =api.getcategoryLowers().Where(i => i.ID == _ID).Select(i => i.Name).FirstOrDefault();
            int cLowerID = api.getcategoryLowers().Where(i => i.ID == _ID).Select(i => i.ID).FirstOrDefault();

            ViewBag.cName = cName;
            ViewBag.cLowerTitleName = cLowerTitleName;
            ViewBag.cLowerame = cLowerame;
            ViewBag.cLowerID = cLowerID;

            ProductListViewModel pVM = new ProductListViewModel(list, cList);
            brands.Clear();
            prices.Clear();
            batterys.Clear();
            return View(pVM);
        }

        public ActionResult GetPageData(int pageNum = 1, int pageSize = 8)
        {
            var produtList = api.getProductByCategoryLowerID(_ID);

            var pagedData = Pagination.PagedResult(produtList, pageNum, pageSize);

            var pReturn = funFill();
            pagedData = Pagination.PagedResult(pReturn, pageNum, pageSize);
           
            ViewBag.priceRight = true;
            return PartialView("~/Views/Shared/_ProductPartial.cshtml", pagedData);
        }
        public PagedData<Product> GetPageDataFilter(List<Product> plist,int pageNum = 1, int pageSize = 8)
        {
            return Pagination.PagedResult(plist, pageNum, pageSize);
        }

        static List<string> brands = new List<string>();
        static List<string> prices = new List<string>();
        static List<string> batterys = new List<string>();

        public ActionResult Filter(string name, string id)
        {
            ViewBag.priceRight = true;
            switch (id)
            {
                case "brands":
                    brands.Add(name);
                    break;
                case "prices":
                    prices.Add(name);
                    break;
                case "batterys":
                    batterys.Add(name);
                    break;
            }
            var pReturn = funFill();
            var allReturn = GetPageDataFilter(pReturn);
            return PartialView("~/Views/Shared/_ProductPartial.cshtml", allReturn);
        }

        public ActionResult filterRemove(string name, string id)
        {
            ViewBag.priceRight = true;
            switch (id)
            {
                case "brands":
                    brands.Remove(name);
                    break;
                case "prices":
                    prices.Remove(name);
                    break;
                case "batterys":
                    batterys.Remove(name);
                    break;
            }
            var pReturn = funFill();
            var allReturn = GetPageDataFilter(pReturn);
            return PartialView("~/Views/Shared/_ProductPartial.cshtml", allReturn);
        }

        public List<Product> funFill()
        {
            List<Product> p = api.getProduct();
            List<Product> pFilterOneList = new List<Product>();
            List<Product> pFilterAllList = new List<Product>();
            if (brands.Count != 0)
            {
                foreach (var item in brands)
                {
                    pFilterOneList = p.Where(i => i.Brand == item).ToList();
                    pFilterAllList.AddRange(pFilterOneList);
                }
                p = pFilterAllList;
            }
            if (prices.Count != 0)
            {
                pFilterOneList.Clear();
                foreach (var item in prices)
                {
                    switch (item)
                    {
                        case "1":
                            pFilterOneList.AddRange(p.Where(i => (i.Outlet == true ? (i.OutletPrice >= 1000 && i.OutletPrice < 2000) :
                            ((i.Price >= 1000 && i.Price < 2000)))).ToList());
                            break;
                        case "2":
                            pFilterOneList.AddRange(p.Where(i => (i.Outlet == true ? (i.OutletPrice >= 2000 && i.OutletPrice < 4000) :
                             ((i.Price >= 2000 && i.Price < 4000)))).ToList());
                            break;
                        case "3":
                            pFilterOneList.AddRange(p.Where(i => (i.Outlet == true ? (i.OutletPrice >= 4000 && i.OutletPrice < 9000) :
                           ((i.Price >= 4000 && i.Price < 9000)))).ToList());
                            break;
                        case "4":
                            pFilterOneList.AddRange(p.Where(i => (i.Outlet == true ? (i.OutletPrice >= 9000) :
                             ((i.Price >= 9000)))).ToList());
                            break;
                    }
                }
                p = pFilterOneList;
            }
            if (batterys.Count != 0)
            {
                pFilterOneList.Clear();
                foreach (var item in batterys)
                {
                    switch (item)
                    {
                        case "1":
                            pFilterOneList.AddRange(p.Where(i => (i.battery >= 900 && i.battery < 2400)).ToList());
                            break;
                        case "2":
                            pFilterOneList.AddRange(p.Where(i => (i.battery >= 2400 && i.battery < 3900)).ToList());
                            break;
                        case "3":
                            pFilterOneList.AddRange(p.Where(i => (i.battery >= 3900 && i.battery < 5000)).ToList());
                            break;
                        case "4":
                            pFilterOneList.AddRange(p.Where(i => (i.battery >= 5000)).ToList());
                            break;
                    }
                }
                p = pFilterOneList;
            }
            if (brands.Count == 0 && prices.Count == 0 && batterys.Count == 0)
            {
                p = m.Product.ToList();
            }
            list = p;
            return list;
        }

    }
}