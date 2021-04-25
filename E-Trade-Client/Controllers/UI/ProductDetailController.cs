using MVCexample.Controllers.API;
using MVCexample.Helper;
using MVCexample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCexample.Controllers
{
    [AllowAnonymous]
    public class ProductDetailController : BaseController
    {
        ServisController api = new ServisController();
        // GET: ProductDetail
        public ActionResult Index(int ID)
        {
            Product p = api.getProductById(ID);
            if (p != null)
            {
                List<Product> pListSimilarFirst = api.m.Product.Where(i => i.CategoryLowerID == p.CategoryLowerID).ToList();
                var pListSimilar = Pagination.PagedResult(pListSimilarFirst, 1, 8);

                string categoryName = api.getcategorys().Where(i => i.ID == p.CategoryID).Select(i => i.Name).FirstOrDefault();
                string cLowerTitleName = api.getcategoryLowerTitles().Where(i => i.ID == p.CategoryLowerTitleID).Select(i => i.Name).FirstOrDefault();
                string cLowerame = api.getcategoryLowers().Where(i => i.ID == p.CategoryLowerID).Select(i => i.Name).FirstOrDefault();

                if (SessionInfo.isLogin == true)
                {
                    BasketContent LoginIsBasket = api.m.BasketContent.Where(i => i.ProductID == ID).FirstOrDefault();
                    if (LoginIsBasket != null && LoginIsBasket.Basket.Status == false)
                        ViewBag.isBasket = true;
                }
                else if (SessionInfo.BasketContent != null)
                {
                    BasketContent SessionIsBasket = SessionInfo.BasketContent.Where(i => i.ProductID == ID).FirstOrDefault();
                    if (SessionIsBasket != null)
                        ViewBag.isBasket = true;
                }

                var comments = api.m.Comment.Where(i => i.ProductID == p.ID).ToList();

                ViewBag.categoryName = categoryName;
                ViewBag.cLowerTitleName = cLowerTitleName;
                ViewBag.cLowerame = cLowerame;
                ViewBag.pListSimilar = pListSimilar;
                ViewBag.swiperMarginZero = true;
                ViewBag.comments = comments;
                return View(p);
            }else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public JsonResult AddBasket(int ID)
        {
            BasketContent BasketContent = new BasketContent();
            if (SessionInfo.ID > 0)
            {
                var basketStatusisFalse = api.m.Basket.Where(i => i.Status != true).FirstOrDefault();
                if (basketStatusisFalse != null)
                {
                    BasketContent.BasketID = basketStatusisFalse.ID;
                    BasketContent.ProductID = ID;
                    BasketContent.Count = 1;
                    m.BasketContent.Add(BasketContent);
                    m.SaveChanges();
                    SessionInfo.BasketCount = api.m.BasketContent.Where(i => i.BasketID == basketStatusisFalse.ID).Count();
                }
                else
                {
                    Basket Basket = new Basket();
                    Basket.accountID = SessionInfo.ID;
                    m.Basket.Add(Basket);
                    m.SaveChanges();
                    var BasketCurrent = api.m.Basket.Where(i => i.Status != true).FirstOrDefault();
                    BasketContent.BasketID = BasketCurrent.ID;
                    BasketContent.ProductID = ID;
                    BasketContent.Count = 1;
                    m.BasketContent.Add(BasketContent);
                    m.SaveChanges();
                    SessionInfo.BasketCount = 1;
                }

            }
            else
            {
                if (SessionInfo.BasketContent != null)
                    SessionInfo.BasketContent.Add(new BasketContent { ProductID = ID });
                else
                {
                    SessionInfo.BasketContent = new List<BasketContent>();
                    SessionInfo.BasketContent.Add(new BasketContent { ProductID = ID });
                }
                SessionInfo.BasketCount = SessionInfo.BasketContent.Count();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}