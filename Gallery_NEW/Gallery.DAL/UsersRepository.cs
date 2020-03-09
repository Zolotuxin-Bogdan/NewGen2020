using Gallery.DAL.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Gallery.DAL
{
    public class UsersRepository : IRepository
    {
        private readonly UserContext _ctx;

        public UsersRepository(UserContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }
        public async Task<bool> IsUserExistAsync(string username, string password)
        {
            return await _ctx.Users.AnyAsync(u => u.EMail == username.Trim().ToLower() &&
                                             u.Password == username.Trim());
        }
    }
}
