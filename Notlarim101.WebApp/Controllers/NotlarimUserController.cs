﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;


namespace Notlarim101.WebApp.Controllers
{
    public class NotlarimUserController : Controller
    {
        private NotlarimUserManager num = new NotlarimUserManager();
        public ActionResult Index()
        {
            return View(num.List());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlarimUser notlarimUser = num.Find(s => s.Id == id);
            if (notlarimUser == null)
            {
                return HttpNotFound();
            }
            return View(notlarimUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //kötü niyetli kişilerin url kullanarak id almalarını engelliyor.
        public ActionResult Create(NotlarimUser notlarimUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                //username kontrolü ve email kontrolü yapılmalı
                BusinessLayerResult<NotlarimUser> res = num.Insert(notlarimUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(notlarimUser);
                }
                return RedirectToAction("Index");
            }

            return View(notlarimUser);
        }

        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlarimUser notlarimUser = num.Find(s => s.Id == id);
            if (notlarimUser == null)
            {
                return HttpNotFound();
            }
            return View(notlarimUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NotlarimUser notlarimUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<NotlarimUser> res = num.Update(notlarimUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(notlarimUser);
                }
                return RedirectToAction("Index");
            }
            return View(notlarimUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlarimUser notlarimUser = num.Find(s => s.Id == id);
            if (notlarimUser == null)
            {
                return HttpNotFound();
            }
            return View(notlarimUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotlarimUser notlarimUser = num.Find(s => s.Id == id);
            num.Delete(notlarimUser);
            return RedirectToAction("Index");
        }
    }
}
