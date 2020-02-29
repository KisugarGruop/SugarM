using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SugarM.Data;

namespace SugarM.GenerricRepository {
    public class GenericRepository<T> : IGenericRepository<T> where T : class {
        private readonly ApplicationDbContext _context;

        public GenericRepository (ApplicationDbContext context) {
            _context = context;
        }
        public void Create (T t) {
            _context.Set<T> ().Add (t);
            _context.SaveChanges ();
        }

        public List<T> FinBy (Expression<Func<T, bool>> predicate) {
            return _context.Set<T> ().Where (predicate).ToList ();
        }

        public List<T> FindAll () {
            return _context.Set<T> ().ToList ();
        }

        public void Remove (T t) {
            _context.Set<T> ().Remove (t);
            _context.SaveChanges ();
        }

        public void Update (T t) {
            _context.Set<T> ().Update (t);
            _context.SaveChanges ();
        }
    }
}