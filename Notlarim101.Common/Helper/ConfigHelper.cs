using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Common.Helper
{
    public class ConfigHelper
    {
        //public static string Get(string key)
        //{
        //    //Configuration manager web.config dosyası içinde  appSettings içinde  oluşturduğumuz mail dosyalarının keylerine ulaşmak için kullanacağım.
        //    return ConfigurationManager.AppSettings[key]; //

                 
        //}
        //mailport int istedğinzden generic yapı kuruldu.    
        public static T Get<T>(string key)
        {
            //port numarası gibi int bir geri dönüş istenirse bunun için metodu generic hale getirerek gelen tipi istenen tipe değiştirererek göndeririz.
            return (T)Convert.ChangeType (ConfigurationManager.AppSettings[key],typeof(T)); //changetype object çeviriyor
        }

    }
}
