using Gallery.DAL.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.DAL
{
    public class UsersRepository : IRepository
    {
        private readonly GalleryContext _ctx;

        public UsersRepository(GalleryContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }
        public async Task<bool> IsUserExistAsync(string username, string password)
        {
            return await _ctx.Users.AnyAsync(u => u.Email == username.Trim().ToLower() &&
                                             u.Password == password.Trim());
        }

        public async Task RegisterUserToDatabase(string username, string password)
        {
            _ctx.Users.Add(new User { Email = username, Password = password });
            _ctx.SaveChanges();
        }

        public int GetUserId(string userName)
        {
            return _ctx.Users.Where(u => u.Email == userName).Select(u => u.Id).FirstOrDefault();
        }

        public string GetUserName(int id)
        {
            return _ctx.Users.Where(u => u.Id == id).Select(u => u.Email).FirstOrDefault();
        }
    }
}
