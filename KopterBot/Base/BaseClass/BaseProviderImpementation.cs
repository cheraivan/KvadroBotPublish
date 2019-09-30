using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Base.BaseClass
{
    class BaseProviderImpementation<T>:BaseProvider<T> where T:class
    {
        DbSet<T> dbSet;
        public BaseProviderImpementation()
        {
            dbSet = db.Set<T>();
        }
        public async override Task Create(T item)
        {
            if(item == null)
                throw new NullReferenceException();
            dbSet.Add(item);
            await db.SaveChangesAsync(); 
        }
        public async override Task Delete(T item)
        {
            if (item == null)
                throw new NullReferenceException();
            dbSet.Remove(item);
            await db.SaveChangesAsync();
        }
        public async override ValueTask<T> FindById(long id)
        {
            return await dbSet.FindAsync(id);
        }
        public async override Task Update(T item)
        {
            if (item == null)
                throw new NullReferenceException();

            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
        public async override ValueTask<IEnumerable<T>> Get()
        {
            return await dbSet.ToListAsync();
        }
        public async override ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate)
        {
            return await Task.Run(()=> dbSet.AsNoTracking().Where(predicate).ToList());
        }
        public async override ValueTask<T> FirstElement(Func<T, bool> predicate)
        {
            IEnumerable<T> enumarable = await Get(predicate);
            if(enumarable != null)
            {
                return enumarable.ToList()[0];
            }
            return null;
        }
        public async override ValueTask<T> LastElement(Func<T, bool> predicate)
        {
            IEnumerable<T> enumarable = await Get(predicate);
            if (enumarable != null)
            {
                return enumarable.ToList()[enumarable.Count()-1]; // проверить
            }
            return null;
        }
    }
}
