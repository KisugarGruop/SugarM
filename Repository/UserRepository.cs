using System.Linq;
using SugarM.Data;
using SugarM.GenerricRepository;
using SugarM.Models;

namespace SugarM.Repository
{
    public class UserRepository : GenericRepository<UserProfile>, IUserprofileRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public UserProfile GetCompCode(string Userid)
        {
            UserProfile userProfile = new UserProfile();

            userProfile = _context.UserProfile.Where(x => x.ApplicationUserId == Userid).FirstOrDefault();
            return userProfile;
        }
    }
}