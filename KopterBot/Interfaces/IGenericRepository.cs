using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Interfaces
{
    interface IGenericRepository<T> where T:class 
    {
        Task Create(T item);
        ValueTask<T> FindById(long id);
        ValueTask<IEnumerable<T>> Get();
        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
        Task Remove(T item);
        Task Update(T item);
        Task Save();
    }
}
