using Notlarim101.BusinessLayer.Abstract;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.BusinessLayer
{
    public class CategoryManager :ManagerBase<Category>
    {
        //Repository<Category> rcat = new Repository<Category>();
        //public List<Category> GetCategories()
        //{
        //    return rcat.List();
        //}

        public override int Delete(Category obj)
        {
            NoteManager nm = new NoteManager();
            //LikedManager
            //CommentManager bu magagerlarinda new leyeceğiz

            //Kategor ile ilişkili notların silinmesi gerekecek

            foreach (Note note in obj.Notes.ToList())
            {
                //Note ile ilişkili Likeların silinmesi
                foreach (Liked like in note.Likes.ToList())
                {
                    //delete
                }

                //Note ile ilişkili Commentların silinmesi
                foreach (Comment comment in note.Comments.ToList())
                {
                    //delete
                }
                //nm.Delete(note);
            }

            return base.Delete(obj);
        }
    }
}
