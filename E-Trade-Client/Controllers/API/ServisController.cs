using MVCexample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCexample.Controllers.API
{
    public class ServisController : ApiController
    {
        public  MVCexampleEntities m = new MVCexampleEntities();

        public ServisController()
        {
            m.Configuration.ProxyCreationEnabled = false;
        }


        #region Kategori
        public List<category> getcategorys()
        {
            return m.category.ToList();
        }
        public List<categoryLower> getcategoryLowers()
        {
            return m.categoryLower.ToList();
        }
        public List<categoryLowerTitle> getcategoryLowerTitles()
        {
            return m.categoryLowerTitle.ToList();
        }
        #endregion

        #region Ürün
        public List<Product> getProduct()
        {
            var pList = m.Product.ToList();
            return pList;
        }

        public Product getProductById(int ID)
        {
            var p = m.Product.Where(i => i.ID == ID).FirstOrDefault();
            p.pDetails = m.ProductDetail.Where(i => i.ProductID == p.ID).ToList();
            return p;
        }

        public List<Product> getProductByCategoryLowerID(int ID)
        {
            var pList = m.Product.Where(i => i.CategoryLowerID == ID).ToList();
            return pList;
        }
        #endregion



        public registerTable getAccount(int ID)
        {
            var r = m.registerTable.Where(i => i.ID == ID).FirstOrDefault();
            return r;
        }

        #region Basket
        public List<Basket> getBasket(int ID)
        {
            var b = m.Basket.Where(i => i.accountID == ID).ToList();
            return b;
        }

        public List<BasketContent> getBasketContent(int ID)
        {
            var b = m.BasketContent.Where(i => i.BasketID == ID).ToList();
            return b;
        } 
        #endregion

        public List<AddressTablee> getAddressTableTrue(int ID)
        {
            var a = m.AddressTablee.Where(i => i.accountID == ID && i.Status == true).ToList();
            return a;
        }

        public List<AddressTablee> getAddressTableFalseOrNull(int ID)
        {
            var a = m.AddressTablee.Where(i => i.accountID == ID && i.Status != false).ToList();
            return a;
        }

        public int postDeleteAddress(int ID)
        {
            var a = m.AddressTablee.Where(i => i.ID == ID).FirstOrDefault();
            if (a != null)
            {
                a.Status = false;
                m.SaveChanges();
                return 1;
            }
            else
                return 0;
        }

    
        [HttpPost]
        public List<Product> findByUserId(int id)
        {
            m.Configuration.ProxyCreationEnabled = false;
            var db = m.Product.Where(w => w.ID == id).ToList();
            return db;
        }

    }
}