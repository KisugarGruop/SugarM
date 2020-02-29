using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SugarM.GenerricRepository {
    public interface IGenericRepository<T> where T : class {
        void Create (T t);
        void Update (T t);
        void Remove (T t);
        List<T> FindAll ();
        List<T> FinBy (Expression<Func<T, bool>> predicate);
    }
}