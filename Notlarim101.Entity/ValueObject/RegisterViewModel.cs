using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notlarim101.Entity.ValueObject
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} alanı boş geçilemez."), 
         StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }
        [DisplayName("Email"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(100, ErrorMessage = "{0} max. {1} karakter olmalı."),
            EmailAddress(ErrorMessage ="{0} alanı için geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(30,MinimumLength =3, ErrorMessage = "{0} max. {1} ile min {2} karakter arasında olmalı.")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"),
           Required(ErrorMessage = "{0} alanı boş geçilemez."),
           DataType(DataType.Password),
           StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı."),Compare("Password",ErrorMessage ="{0} ile {1} uyuşmuyor")]
        public string RePassword { get; set; }



        /*
         DATA ANNOTATİONS ARAŞTIRRRRRRRRR !!!!!!!!!!!!!!!!!!
         */
    }
}