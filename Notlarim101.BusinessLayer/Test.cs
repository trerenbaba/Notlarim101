using Notlarim101.DataAccessLayer;
using Notlarim101.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.BusinessLayer
{
    public class Test
    {
        Repository<NotlarimUser> ruser = new Repository<NotlarimUser>();
        Repository<Category> rcat = new Repository<Category>();
        Repository<Comment> rcom = new Repository<Comment>();
        Repository<Note> rnote = new Repository<Note>();
        Repository<Liked> rlike = new Repository<Liked>();
        public Test()
        {
            //her bir classın constructor ı vardır.
            //NotlarimContext db = new NotlarimContext();
            //db.NotlarimUsers.ToList();
            var test = rcat.List();
            var test1 = rcat.List(x => x.Id > 5);


            test1.Where(x => x.Id > 3).ToList();
            var test2 = rcat.QList(x => x.Id > 5);
            test2.ToList();
        }

    }
}
