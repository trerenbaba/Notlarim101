using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Common
{
    public class DefaultCommon : ICommon
    {
        //UI katmanı ilk çalıştıtığımda bu classı new leyecek
        public string GetCurrentUsername()
        {
            return "system";
        }
    }
}
