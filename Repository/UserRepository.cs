using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SugarM.Data;
using SugarM.GenerricRepository;
using SugarM.Models;

namespace SugarM.Repository {
    public class UserRepository : GenericRepository<UserProfile>, IUserprofileRepository {
        private readonly ApplicationDbContext _context;
        public UserRepository (ApplicationDbContext context) : base (context) {
            _context = context;
        }
        public async Task<UserProfile> GetUserProfile (string Userid) {
            UserProfile userProfile = new UserProfile ();
            userProfile = await _context.UserProfile.Where (x => x.ApplicationUserId == Userid).FirstOrDefaultAsync ();
            return userProfile;
        }
    }
}