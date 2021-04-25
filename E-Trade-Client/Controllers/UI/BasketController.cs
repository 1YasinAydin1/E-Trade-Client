using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCexample.Models;
using MVCexample.Helper;
using MVCexample.Controllers.API;

namespace MVCexample.Controllers
{
    [AllowAnonymous]
    public class BasketController : BaseController
    {
        static int basketAcceptID = 0;
        public ActionResult Index()
        {
            List<BasketContent> BasketContents = new List<BasketContent>();
            List<Product> pList = new List<Product>();
            if (SessionInfo.BasketContent != null)
                BasketContents.AddRange(SessionInfo.BasketContent);
            if (SessionInfo.isLogin == true)
            {
                var control = m.Basket.Where(i => i.accountID == SessionInfo.ID && i.Status != true).FirstOrDefault();
                if (control != null)
                {
                    basketAcceptID = control.ID;
                    //BasketContents.Clear();
                    BasketContents.AddRange(m.BasketContent.Where(i => i.BasketID == control.ID).ToList());
                }
                ViewBag.address = m.AddressTablee.Where(i => i.accountID == SessionInfo.ID && i.Status!=true).ToList();
            }
            foreach (var item in BasketContents)
                pList.AddRange(m.Product.Where(i => i.ID == item.ProductID).ToList());
            return View(pList);
        }

        public int isLogin(string[] PriceArray, int total, int addressID)
        {
            //var servis = new ServisController();
            //var snc = servis.getMesaj();

            var arrayPrice = PriceArray[0].Split(',');

            if (SessionInfo.isLogin)
            {
                Basket updateBasket = m.Basket.Where(i => i.ID == basketAcceptID).FirstOrDefault();
                if (updateBasket != null)
                {
                    updateBasket.orderDate = DateTime.Now;
                    updateBasket.totalPrice = total;
                    updateBasket.Status = true;
                    updateBasket.AddressID = addressID;

                    var basketContents = m.BasketContent.Where(i => i.BasketID == basketAcceptID).ToList();

                    for (int i = 0; i < basketContents.Count; i++)
                    {
                        if (basketContents[i].Product.Outlet == true)
                        {
                            var price = int.Parse(basketContents[i].Product.OutletPrice.ToString());
                            var totalPrice = int.Parse(arrayPrice[i]);
                            basketContents[i].Count = totalPrice / price;
                        }
                        else
                        {
                            var price = int.Parse(basketContents[i].Product.Price.ToString());
                            var totalPrice = int.Parse(arrayPrice[i]);
                            basketContents[i].Count = totalPrice / price;
                        }
                    }
                    if (SessionInfo.BasketContent!=null)
                    if (SessionInfo.BasketContent.Count!=0)
                    {
                        BasketContent BasketContent = new BasketContent();
                        for (int i = 0; i < SessionInfo.BasketContent.Count; i++)
                        {
                            var item = SessionInfo.BasketContent[i];
                            BasketContent.BasketID = updateBasket.ID;
                            BasketContent.ProductID = item.ProductID;
                            BasketContent.Count = 1;

                            var isOutlet = m.Product.Where(k => k.ID == item.ProductID).FirstOrDefault();
                            if (isOutlet != null)
                            {
                                if (isOutlet.Outlet == true)
                                {
                                    var price = int.Parse(isOutlet.OutletPrice.ToString());
                                    var totalPrice = int.Parse(arrayPrice[i]);
                                    BasketContent.Count = totalPrice / price;
                                }
                                else
                                {
                                    var price = int.Parse(isOutlet.Price.ToString());
                                    var totalPrice = int.Parse(arrayPrice[i]);
                                    BasketContent.Count = totalPrice / price;
                                }
                            }
                            m.BasketContent.Add(BasketContent);
                        }
                    }
                    m.SaveChanges();
                }
                else
                {
                    Basket Basket = new Basket();
                    BasketContent BasketContent = new BasketContent();
                    Basket.accountID = SessionInfo.ID;
                    Basket.orderDate = DateTime.Now;
                    Basket.AddressID = addressID;
                    Basket.totalPrice = total;
                    m.Basket.Add(Basket);
                    m.SaveChanges();
                    var BasketCurrent = m.Basket.Where(i => i.Status != true).FirstOrDefault();
                    for (int i = 0; i < SessionInfo.BasketContent.Count; i++)
                    {
                        var item = SessionInfo.BasketContent[i];
                        BasketContent.BasketID = BasketCurrent.ID;
                        BasketContent.ProductID = item.ProductID;
                        BasketContent.Count = 1;

                        var isOutlet = m.Product.Where(k => k.ID == item.ProductID).FirstOrDefault();
                        if (isOutlet != null)
                        {
                            if (isOutlet.Outlet == true)
                            {
                                var price = int.Parse(isOutlet.OutletPrice.ToString());
                                var totalPrice = int.Parse(arrayPrice[i]);
                                BasketContent.Count = totalPrice / price;
                            }
                            else
                            {
                                var price = int.Parse(isOutlet.Price.ToString());
                                var totalPrice = int.Parse(arrayPrice[i]);
                                BasketContent.Count = totalPrice / price;
                            }
                        }
                        m.BasketContent.Add(BasketContent);
                    }

                    Basket.Status = true;
                    SessionInfo.BasketContent = null;
                    SessionInfo.basketIsFill = false;
                    m.SaveChanges();
                }
                SessionInfo.BasketCount = 0;
                return 1;
            }
            else
            {
                SessionInfo.basketIsFill = true;
                return 0;
            }
        }
    }
}