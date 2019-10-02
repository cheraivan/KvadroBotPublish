using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KopterBot.Base
{
    interface IBaseProvider<T>
    {
        ValueTask<T> FindById(long id);
        Task Update(T item);
        Task Delete(T item);
        Task Create(T item);
        IQueryable<T> Get();

        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
    }
}
