using MVCexample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCexample.Helper
{
    public static class SessionInfo
    {
        public static bool isLogin
        {
            get
            {
                try
                {
                    return (bool)HttpContext.Current.Session["isLogin"];
                }
                catch (Exception)
                {
                    return false;
                }

            }
            set { HttpContext.Current.Session["isLogin"] = value; }
        }
        public static string UserName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["UserName"].ToString();
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            }
            set { HttpContext.Current.Session["UserName"] = value; }
        }
        public static int ID
        {
            get
            {
                try
                {
                    return (int)HttpContext.Current.Session["ID"];
                }
                catch (Exception)
                {
                    return 0;
                }

            }
            set { HttpContext.Current.Session["ID"] = value; }
        }
        public static int BasketCount
        {
            get
            {
                try
                {
                    return (int)HttpContext.Current.Session["BasketCount"];
                }
                catch (Exception)
                {
                    return 0;
                }

            }
            set { HttpContext.Current.Session["BasketCount"] = value; }
        }
        public static bool basketIsFill
        {
            get
            {
                try
                {
                    return (bool)HttpContext.Current.Session["basketIsFill"];
                }
                catch (Exception)
                {
                    return false;
                }

            }
            set { HttpContext.Current.Session["basketIsFill"] = value; }
        }
        public static List<BasketContent> BasketContent
        {
            get
            {
                List<BasketContent> empty = new List<BasketContent>();
                try
                {
                    return (List<BasketContent>)HttpContext.Current.Session["BasketContent"];
                }
                catch (Exception)
                {
                    return empty;
                }

            }
            set { HttpContext.Current.Session["BasketContent"] = value; }
        }

        public static void Clear()
        {

            Type myType = typeof(SessionInfo);
            System.Reflection.PropertyInfo[] properties = myType.GetProperties(
                   System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                property.SetValue(property.Name, null);
            }
        }

    }
}