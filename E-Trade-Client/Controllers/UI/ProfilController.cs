using MVCexample.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCexample.Models;
using MVCexample.Controllers.API;

namespace MVCexample.Controllers
{
    [Authorize]
    public class ProfilController : BaseController
    {
        public ActionResult Index()
        {
            var account = m.registerTable.Where(i => i.ID == SessionInfo.ID).FirstOrDefault();
            List<Basket> baskets = m.Basket.Where(i => i.accountID == SessionInfo.ID).ToList();

            List<BasketContent> basketContents = new List<BasketContent>();

            foreach (var item in baskets)
            {
                var content = m.BasketContent.Where(i => i.BasketID == item.ID).ToList();
                basketContents.AddRange(content);
            }
            ViewBag.address = m.AddressTablee.Where(i => i.accountID == SessionInfo.ID && i.Status == true).ToList();
            ViewBag.addressTrue = m.AddressTablee.Where(i => i.accountID == SessionInfo.ID && i.Status != false).ToList();
            ViewBag.baskets = baskets;
            ViewBag.basketContents = basketContents;
            return View(account);
        }

        public PartialViewResult CommentPartial()
        {
            return PartialView();
        }

        public PartialViewResult OrderPartial()
        {
            return PartialView();
        }

        public PartialViewResult ProfilPartial()
        {
            return PartialView();
        }

        public int ProfileUpdate(string name, string lastname, string username, string mail)
        {
            try
            {
                var UpdateAccount = m.registerTable.Where(i => i.ID == SessionInfo.ID).FirstOrDefault();

                UpdateAccount.name = name;
                UpdateAccount.lastname = lastname;
                UpdateAccount.username = username;
                UpdateAccount.mail = mail;
                SessionInfo.UserName = username;
                m.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public int ProfilePasswordUpdate(string currentPassword, string newPassword)
        {

            try
            {
                string passwordCurrentMD5 = encryption.MD5Olustur(currentPassword);
                string passwordNewMD5 = encryption.MD5Olustur(newPassword);
                var UpdateAccount = m.registerTable.Where(i => i.ID == SessionInfo.ID).FirstOrDefault();
                if (UpdateAccount != null)
                {
                    if (UpdateAccount.password != passwordCurrentMD5)
                        return 0;
                    else
                    {
                        UpdateAccount.password = passwordNewMD5;
                        m.SaveChanges();
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public PartialViewResult AddressPartial()
        {
            return PartialView();
        }
        public int AddressAdd(string Parish, string District, string City, string address, string Title)
        {
            AddressTablee a = new AddressTablee();
            a.Parish = Parish;
            a.District = District;
            a.City = City;
            a.address = address;
            a.Title = Title;
            a.accountID = SessionInfo.ID;
            m.AddressTablee.Add(a);
            m.SaveChanges();
            return a.ID;
        }
        public int AddressDelete(int ID)
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

        public int AddressUpdate(string Parish, string District, string City, string address, string Title, int ID)
        {
            try
            {
                var UpdateAddress = m.AddressTablee.Where(i => i.ID == ID).First();

                UpdateAddress.Parish = Parish;
                UpdateAddress.District = District;
                UpdateAddress.City = City;
                UpdateAddress.address = address;
                UpdateAddress.Title = Title;

                m.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }


        public int AddComment(int star, string comment, int pID, int basketContentID)
        {
            try
            {
                Comment c = new Models.Comment();
                c.AccountID = SessionInfo.ID;
                c.ProductID = pID;
                c.StarCount = byte.Parse(star.ToString());
                c.CommentText = comment;
                m.Comment.Add(c);
                m.SaveChanges();

                var bContent = m.BasketContent.Where(i => i.ID == basketContentID).FirstOrDefault();
                if (bContent != null)
                {
                    bContent.CommentStatus = true;
                    bContent.CommentID = c.ID;
                }

                m.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}