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
    public class NotlarimUserManager:ManagerBase<NotlarimUser>
    {
        //Kullanıcı username kontrolü yapmalıyım.
        //kullanıcı email kontrolü yapmalıyım.
        //kayıt işlemini gercekleştirmeliyim.
        //Activasyon e-postası gönderimi yapmalıyım. 

        BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //Giris kontrolü
            //Hesap aktif edilmiş mi kontrolü

            res.Result = Find(s => s.UserName == data.UserName && s.Password == data.Password);
            if (res.Result != null)
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

        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
           NotlarimUser user= Find(s => s.UserName == data.Username || s.Email==data.Email);
            if (user!=null)
            {
                if (user.UserName==data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email kayıtlı.");
                }
                //throw new Exception("Kayıtlı kullanıcı yada e-posta adresi");
            }
            else
            {
                int dbResult = base.Insert(new NotlarimUser()
                {
                    Name=data.Name,
                    Surname=data.SurName,
                    UserName = data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ProfileImageFilename="User.png",
                    ActivateGuid=Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,
                    //repository e taşındı
                    //ModifiedOn=DateTime.Now,
                    //CreatedOn=DateTime.Now,
                    //ModifiedUsername="system"
                    
                });;

                if (dbResult > 0)
                {
                    res.Result = Find(s => s.Email == data.Email && s.UserName == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.UserName}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayın</a>.";
                    MailHelper.SendMail(body, res.Result.Email, "Notlarim101 hesap aktifleştirme");

                        //activasyon maili atılacak
                        //lr.Result.ActivateGuid;
                }
            }

            return res;
        }
  
        public BusinessLayerResult<NotlarimUser> ActivateUser(Guid id) //Route.config id yazdığı için id yazıyoruz. Başka isim yazarsak kabul etmiyor.
        {
            res.Result = Find(x => x.ActivateGuid == id);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha önce aktif edilmiş!!!");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Sal beni!!");
            }
            return res;

        }

        public BusinessLayerResult<NotlarimUser> GetUserById(int id)
        {
            res.Result = Find(s => s.Id == id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> UpdateProfile(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.Id != data.Id && (s.UserName==data.UserName || s.Email==data.Email));
            if (user!=null && user.Id !=data.Id)
            {
                if (user.UserName==data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanıcı adı daha önce kaydedilmiş.");
                }
                if (user.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha önce kaydedilmiş.");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.UserName = data.UserName;
            res.Result.Password = data.Password;
            if (!string.IsNullOrEmpty(data.ProfileImageFilename))
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }
            if (base.Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil Güncellenemedi");
            }

            return res;

        }

        public BusinessLayerResult<NotlarimUser> RemoveUserById(int id)
        {
            NotlarimUser user = Find(s => s.Id==id);
            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }
            return res;
        }
   

        //Hiding Method...
        public new BusinessLayerResult<NotlarimUser> Insert(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.UserName == data.UserName || s.Email == data.Email);
            res.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email kayıtlı.");
                }
                //throw new Exception("Kayıtlı kullanıcı yada e-posta adresi");
            }
            else
            {
                res.Result.ProfileImageFilename = "User1.png";
                res.Result.ActivateGuid = Guid.NewGuid();


                if (base.Insert(res.Result)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<NotlarimUser> Update(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.Id != data.Id && (s.UserName == data.UserName || s.Email == data.Email));

            if (user != null && user.Id != data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanıcı adı daha önce kaydedilmiş.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha önce kaydedilmiş.");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.UserName = data.UserName;
            res.Result.Password = data.Password;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");
            }

            return res;
        }
    }
}
