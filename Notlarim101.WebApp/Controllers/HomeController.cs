using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;
using Notlarim101.WebApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Notlarim101.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CategoryManager cm = new CategoryManager();
        private NotlarimUserManager num = new NotlarimUserManager();
        private BusinessLayerResult<NotlarimUser> res;

        // GET: Home
        public ActionResult Index()
        {
            //Test test = new Test();
            ////test.InsertTest();
            ////test.UpdateTest();
            ////test.DeleteTest();
            //test.CommentTest();

            NoteManager nm = new NoteManager();

            return View(nm.QList().OrderByDescending(s => s.ModifiedOn).ToList());
        }


        public ActionResult ByCategoryId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Note> notes = nm.QList().Where(x => x.IsDraft == false && x.Category.Id == id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                res = num.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "https://Home/UserActivate/1234-2345-234567";
                    }
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }
                //yönlendirme

                Session["login"] = res.Result; //session a kullanıcı bilgilerini aktarma
                return RedirectToAction("Index"); //yonlendirme

            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //bool hasError = false;
            if (ModelState.IsValid)
            {
                res = num.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }

                //try
                //{
                //    user = num.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);
                //}
                //if (user==null)
                //{
                //    return View(model);
                //}
                //return RedirectToAction("RegisterOk");
                //if (model.Username=="aaa")
                //{
                //    ModelState.AddModelError("","Kullanıcı adı kullanılıyor.");
                //    //hasError = true;
                //}
                //if (model.Email=="aaa@aaa.com")
                //{
                //    ModelState.AddModelError("", "Bu email kullanılıyor.");
                //    //hasError = true;
                //}

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count>0)
                //    {
                //        return View(model);
                //    }
                //}
                //return RedirectToAction("RegisterOk");

                //if (hasError==true)
                //{
                //    return View(model);
                //}
                //else
                //{
                //    return RedirectToAction("RegisterOk");
                //}

                OkViewModel notifyObj = new OkViewModel
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyObj);
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {

            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            res = num.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }

        public ActionResult UserActivateOk()
        {

            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult ShowProfile()
        {
            //NotlarimUser currentUser = Session["login"] as NotlarimUser;
            //if (currentUser!=null)
            //{
            //    res = num.GetUserById(currentUser.Id);
            //}
            if (Session["login"] is NotlarimUser currentUser)
                res = num.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
                //kullanıcıyı bir hata ekranına yönlendireceğim.
            }

            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            if (Session["login"] is NotlarimUser currentUser)
                res = num.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors,
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(NotlarimUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"
                    ))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                res = num.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profile Güncellenemedi!!!",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            if (Session["login"] is NotlarimUser currentUser)
                res = num.RemoveUserById(currentUser.Id);
      
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi",
                    Items = res.Errors,
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult DeleteProfile(int id)
        //{
        //    return View();
        //}

        //public ActionResult TestNotify()
        //{
        //    ErrorViewModel model = new ErrorViewModel()
        //    {
        //        Heeader = "Yönlendirme",
        //        Title = "Başaraılı",
        //        RedirectingTimeout = 10000,
        //        Items = new List<ErrorMessageObj>() {
        //            new ErrorMessageObj(){Message="Test başarılı 1"},
        //            new ErrorMessageObj(){Message="Test başarılı 2"},
        //        }
        //    };
        //    return View("Error", model);
        //}

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
