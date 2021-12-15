using Notlarim101.DataAccessLayer;
using Notlarim101.DataAccessLayer.EntityFramework;
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
            //var test = rcat.List();
            //var test1 = rcat.List(x => x.Id > 5);
            

            //test1.Where(x => x.Id > 3).ToList();
            //var test2 = rcat.QList(x => x.Id > 5);
            //test2.ToList();
        }
        public void InsertTest()
        {
            int result = ruser.Insert(new NotlarimUser() { 
                Name="Abuzittin",
                Surname="Zerdali",
                Email="abuzer@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive=true,
                IsAdmin=false,
                UserName="abuzer",
                Password="123",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now,
                ModifiedUsername="abuzer"
            }); ;
        }
        public void UpdateTest()
        {
            NotlarimUser user = ruser.Find(s => s.UserName == "abuzer");
            if (user!=null)
            {
                user.Password = "11111";
                ruser.Update(user);
            }
        }
        public void DeleteTest()
        {
            NotlarimUser user = ruser.Find(s => s.UserName == "abuzer");
            if (user != null)
            {
                ruser.Delete(user);
            }
        }
        public void CommentTest()
        {
            NotlarimUser user = ruser.Find(s => s.Id == 1);
            Note note = rnote.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "Bu bir test yorumudur.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "erenbaba",
                Note = note,
                Owner = user
            };
            rcom.Insert(comment);
        }

    }
}
