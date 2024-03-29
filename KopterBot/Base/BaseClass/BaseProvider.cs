﻿using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Base
{
    class BaseProvider<T>:DbProvider<T>,IBaseProvider<T>
    {
        
        public virtual ValueTask<T> FindById(long id)
        {
            throw new NotImplementedException("method has to be override");
        }

        public virtual Task Update(T item)
        {
            throw new NotImplementedException("method has to be override");
        }

        public virtual Task Delete(T item)
        {
            throw new NotImplementedException("method has to be override");
        }

        public virtual Task Create(T item)
        {
            throw new NotImplementedException("method has to be override");
        }

        public virtual IQueryable<T> Get()
        {
            throw new NotImplementedException("method has to be override");
        }

        public virtual ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate)
        {
            throw new NotImplementedException("method has to be override");
        }
    }
}

