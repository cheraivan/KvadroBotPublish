using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Interfaces
{
    interface IRepository<T> where T:class
    {
        ValueTask<T> FindById(long id);
        Task Update(T item);
        Task Delete(T item);
        Task Create(T item);
    }
}
