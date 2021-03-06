using Notlarim101.Common;
using Notlarim101.Core.DataAccess;
using Notlarim101.DataAccessLayer;
using Notlarim101.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase,IDataAccess<T> where T : class // T nesnesi referans type  olmalıdır.Classlar da referans type olduğu için kısıtlamak için kullanılmıştır.
    {
        private DbSet<T> objSet;

        public Repository()
        {
            objSet = db.Set<T>();
        }

        public List<T> List()
        {
            //Set(içine sadece referans tipler verilir)
            return objSet.ToList();
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return objSet.Where(where).ToList();
        }
        public IQueryable<T> QList()
        {
            return objSet.AsQueryable<T>(); //AsQueryable queryable gibi davran demekmiş.
        } 

        public int Insert(T obj)
        {
            objSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase; // ARAŞTIRRRR
                
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername(); //"system"; // common katmanında sessiondan alıp buraya getirdik.
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername(); //"system"; // common katmanında sessiondan alıp buraya getirdik.
            }
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
        public T Find(Expression<Func<T, bool>> find)
        {
            return objSet.FirstOrDefault(find);
        }
    }
}
