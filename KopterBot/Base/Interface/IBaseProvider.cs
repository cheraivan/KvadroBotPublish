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

        ValueTask<T> FirstElement(Func<T, bool> predicate);
        ValueTask<T> LastElement(Func<T, bool> predicate);

        IQueryable<T> Get();

        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
    }
}
