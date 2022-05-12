using System.Collections.Generic;
using System.Threading.Tasks;
using ProAgil.Domain.Identity;

namespace ProAgil.Persistence.Contratos
{
    public interface IUserPersist : IGeralPersist
    {
         Task<IEnumerable<User>> GetUsersAsync();

         Task<User> GetUserByIdAsync(int id);

         Task<User> GetUserByUsernameAsync(string username);
    }
}