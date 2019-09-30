using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KopterBot.Base
{
    interface IBaseProvider<T>
    {
        ValueTask<T> FindById(long id);
        Task Update(T item);
        Task Delete(T item);
        Task Create(T item);

        ValueTask<IEnumerable<T>> Get();

        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
    }
}
