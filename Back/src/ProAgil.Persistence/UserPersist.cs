using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Identity;
using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Contratos;

namespace ProAgil.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly ProAgilContext context;

        public UserPersist(ProAgilContext context): base(context)
        {
            this.context = context;

        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(u=>u.UserName == username.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }
    }
}