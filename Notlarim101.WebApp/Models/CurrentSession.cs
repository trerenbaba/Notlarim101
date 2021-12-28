﻿using Notlarim101.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notlarim101.WebApp.Models
{
    public class CurrentSession
    {
        private int myVar;

        public static NotlarimUser User
        {
            get
            {
                return Get<NotlarimUser>("login");
                //if (HttpContext.Current.Session["login"]!=null)
                //{
                //    return HttpContext.Current.Session["login"] as NotlarimUser;
                //}
                //return null;
            }
        }

        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T) HttpContext.Current.Session[key];
            }
            return default(T);
        }
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

    }
}