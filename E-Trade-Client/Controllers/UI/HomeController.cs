using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCexample.Models;
using MVCexample.Helper;
using MVCexample.Controllers.API;

namespace MVCexample.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        ServisController api = new ServisController();
        [HttpGet]
        public ActionResult Index()
        {
            List<category> saf = (List<category>)Session["cMom"];
            List<Product> p = api.getProduct();
            ViewBag.Home = true;
            ProductCategoryViewModels pc = new ProductCategoryViewModels(p);

            var allReturn = GetPageDataFilter(p);
            return View(allReturn);
        }

        public PagedData<Product> GetPageDataFilter(List<Product> plist, int pageNum = 1, int pageSize = 8)
        {
            var pagedData = Pagination.PagedResult(plist, pageNum, pageSize);
            return pagedData;
        }

        [HttpPost]
        public ActionResult Index(string p)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "RegisterLogin");
        }
    }
}