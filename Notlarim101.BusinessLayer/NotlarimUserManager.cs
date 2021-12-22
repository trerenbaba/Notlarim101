using Notlarim101.BusinessLayer.Abstract;
using Notlarim101.Common.Helper;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.BusinessLayer
{
    public class NotlarimUserManager
    {
        //Kullanıcı username kontrolü yapmalıyım.
        //kullanıcı email kontrolü yapmalıyım.
        //kayıt işlemini gercekleştirmeliyim.
        //Activasyon e-postası gönderimi yapmalıyım.

        Repository<NotlarimUser> ruser = new Repository<NotlarimUser>();
        
        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
           NotlarimUser user= ruser.Find(s => s.UserName == data.Username && s.Email==data.Email);
            BusinessLayerResult<NotlarimUser> lr = new BusinessLayerResult<NotlarimUser>();
            if (user!=null)
            {
                if (user.UserName==data.Username)
                {
                    lr.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email==data.Email)
                {
                    lr.AddError(ErrorMessageCode.EmailAlreadyExist, "Email kayıtlı.");
                }
                //throw new Exception("Kayıtlı kullanıcı yada e-posta adresi");
            }
            else
            {
                int dbResult = ruser.Insert(new NotlarimUser()
                {
                    Name=data.Name,
                    Surname=data.SurName,
                    UserName = data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,
                    //repository e taşındı
                    //ModifiedOn=DateTime.Now,
                    //CreatedOn=DateTime.Now,
                    //ModifiedUsername="system"
                    
                });

                if (dbResult > 0)
                {
                    lr.Result = ruser.Find(s => s.Email == data.Email && s.UserName == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{lr.Result.ActivateGuid}";
                    string body = $"Merhaba {lr.Result.UserName}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayın</a>.";
                    MailHelper.SendMail(body, lr.Result.Email, "Notlarim101 hesap aktifleştirme");

                        //activasyon maili atılacak
                        //lr.Result.ActivateGuid;
                }
            }

            return lr;
        }

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //Giris kontrolü
            //Hesap aktif edilmiş mi kontrolü
           
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(s => s.UserName == data.UserName || s.Password == data.Password);
            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                   
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı adı aktifleştirilmemiş!!!");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen mailinizi kontrol edin.");
                    
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "kullanıcı adı yada şifre uyuşmuyor.");
            }
            return res;

        }

        public BusinessLayerResult<NotlarimUser> ActivateUser(Guid id) //Route.config id yazdığı için id yazıyoruz. Başka isim yazarsak kabul etmiyor.
        {
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(x => x.ActivateGuid == id);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha önce aktif edilmiş!!!");
                    return res;
                }
                res.Result.IsActive = true;
                ruser.Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Sal beni!!");
            }
            return res;

        }

    }
}
