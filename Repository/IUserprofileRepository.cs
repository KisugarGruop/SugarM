using System.Threading.Tasks;
using SugarM.GenerricRepository;
using SugarM.Models;

namespace SugarM.Repository {
    public interface IUserprofileRepository : IGenericRepository<UserProfile> {
        Task<UserProfile> GetUserProfile (string Userid);
    }
}