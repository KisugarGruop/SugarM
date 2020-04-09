using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SugarM.GenerricRepository {
    public interface IGenericRepository<T> where T : class {
        Task Create (T t);
        Task CreateRange (List<T> t);
        Task Update (T t);
        Task UpdateRange (List<T> t);
        Task Remove (T t);
        Task RemoveRange (List<T> t);
        Task<List<T>> FindAll ();
        Task<List<T>> FindBy (Expression<Func<T, bool>> predicate);

    }
}