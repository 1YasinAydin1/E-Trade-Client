using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCexample.Models;
using System.Web.Security;
using MVCexample.Helper;
using MVCexample.Controllers.API;

namespace MVCexample.Controllers
{
    public class BaseController : Controller
    {
        public  MVCexampleEntities m = new MVCexampleEntities();
        public BaseController()
        {
        ServisController api = new ServisController();
            
            if (System.Web.HttpContext.Current.Session["cMom"]==null)
            {
                System.Web.HttpContext.Current.Session.Add("cMom", api.getcategorys());
                System.Web.HttpContext.Current.Session.Add("cLowerTitle", api.getcategoryLowerTitles());
                System.Web.HttpContext.Current.Session.Add("cLower", api.getcategoryLowers());
            }
            if (SessionInfo.isLogin == true) {
                ViewBag.isLogin = true;
                ViewBag.UserName = SessionInfo.UserName;
                var b = api.m.Basket.Where(i => i.accountID == SessionInfo.ID && i.Status!=true).FirstOrDefault();
                int count = 0;
                if (b != null)
                    count = api.m.BasketContent.Where(i => i.BasketID == b.ID).Count();
                ViewBag.Basket = count + SessionInfo.BasketCount;
            }
            else if (SessionInfo.BasketContent != null)
                ViewBag.Basket = SessionInfo.BasketCount;
        }
        public ActionResult signOut(string p)
        {
            SessionInfo.Clear();
            FormsAuthentication.SignOut();
            SessionInfo.BasketCount = 0;
            return RedirectToAction("Index", "Home");
        }
    }
}