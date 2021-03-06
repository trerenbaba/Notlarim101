using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.WebApp.Models;

namespace Notlarim101.WebApp.Controllers
{
    public class CommentController : Controller
    {
        CommentManager cmm = new CommentManager();
        NoteManager nm = new NoteManager();
        public ActionResult Index()
        {
            return View(cmm.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s=>s.Id==id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment,int? noteId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (noteId==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Note note = nm.Find(s=>s.Id==noteId);
                if (note==null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = note;
                comment.Owner = CurrentSession.User;
                if (cmm.Insert(comment)>0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

                return RedirectToAction("Index");
            }

            return Json(new {result=false},JsonRequestBehavior.AllowGet);

            
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment==null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if (cmm.Update(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (cmm.Delete(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Comment comment = cm.Find(s => s.Id == id);
        //    db.Comments.Remove(comment);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult ShowNoteComments(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Note note = nm.Find(s => s.Id == id);
            Note note = nm.QList().Include("Comments").FirstOrDefault(s => s.Id == id);
            if (note==null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialComments", note.Comments);
        }

    }
}
