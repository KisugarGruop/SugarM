using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SugarM.Data;

namespace SugarM.GenerricRepository {
    public class GenericRepository<T> : IGenericRepository<T> where T : class {
        private readonly ApplicationDbContext _context;

        public GenericRepository (ApplicationDbContext context) {
            _context = context;
        }
        public async Task Create (T t) {
            _context.Set<T> ().Add (t);
            await _context.SaveChangesAsync ();
        }
        public async Task CreateRange (List<T> t) {
            _context.Set<T> ().AddRange (t);
            await _context.SaveChangesAsync ();
        }
        public async Task<List<T>> FindBy (Expression<Func<T, bool>> predicate) {
            return await _context.Set<T> ().Where (predicate).ToListAsync ();
        }
        public async Task<List<T>> FindAll () {
            return await _context.Set<T> ().ToListAsync ();
        }
        public async Task Remove (T t) {
            _context.Set<T> ().Remove (t);
            await _context.SaveChangesAsync ();
        }
        public async Task RemoveRange (List<T> t) {
            _context.Set<T> ().RemoveRange (t);
            await _context.SaveChangesAsync ();
        }
        public async Task Update (T t) {
            _context.Set<T> ().Update (t);
            await _context.SaveChangesAsync ();
        }
        public async Task UpdateRange (List<T> t) {
            _context.Set<T> ().UpdateRange (t);
            await _context.SaveChangesAsync ();
        }
    }
}