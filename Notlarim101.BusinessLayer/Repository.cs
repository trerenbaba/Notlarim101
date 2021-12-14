using Notlarim101.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.BusinessLayer
{
    public class Repository<T> where T:class // T nesnesi referans type  olmalıdır.Classlar da referans type olduğu için kısıltlamak için kullanılmıştır.
    {
        NotlarimContext db = new NotlarimContext();
        DbSet<T> objSet;

        public Repository()
        {
            objSet = db.Set<T>();
        }
        
        public List<T> List()
        {
            //Set(içine sadece referans tipler verilir)
            return objSet.ToList();
        }
        public List<T> List(Expression<Func<T,bool>> where)
        {
            return objSet.Where(where).ToList();
        }
        public IQueryable<T> QList(Expression<Func<T,bool>> query)
        {
            return objSet.Where(query);
        }

        public int Insert(T obj)
        {
            objSet.Add(obj);
            return Save();
        }
        public int Update(T obj)
        {
            return Save();
        }
        public int Delete(T obj)
        {
            objSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
