using KopterBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        DbContext db;
        DbSet<T> dbSet;

        public GenericRepository(DbContext db)
        {
            this.db = db;
            dbSet = db.Set<T>();
        }

        public async Task Create(T item)
        {
            dbSet.Add(item);
            await Save();
        }

        public virtual async ValueTask<T> FindById(long id)
        {
            return await dbSet.FindAsync(id);
        }

        public async ValueTask<IEnumerable<T>> Get()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate)
        {
            return await Task.Run(()=>dbSet.AsNoTracking()
                .Where(predicate)
                .ToList());
        }

        public async Task Remove(T item)
        {
            dbSet.Remove(item);
            await Save();
        }

        public async Task Save() =>
            await db.SaveChangesAsync();

        public async Task Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
            await Save();
        }
    }
}
