using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.BusinessLayer
{
    public class CategoryManager
    {
        Repository<Category> rcat = new Repository<Category>();
        public List<Category> GetCategories()
        {
            return rcat.List();
        }
    }
}
