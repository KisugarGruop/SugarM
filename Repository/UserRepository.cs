using SugarM.Data;
using SugarM.GenerricRepository;
using SugarM.Models;

namespace SugarM.Repository {
    public class UserRepository : GenericRepository<UserProfile>, IUserprofileRepository {
        public UserRepository (ApplicationDbContext context) : base (context) {

        }
    }
}