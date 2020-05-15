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

        public async Task<bool> IsUserExistAsync(string username)
        {
            return await _ctx.Users.AnyAsync(u => u.Email == username.Trim().ToLower());
        }

        public async Task RegisterUserToDatabaseAsync(string username, string password)
        {
            _ctx.Users.Add(new User { Email = username, Password = password });
            await _ctx.SaveChangesAsync();
        }

        public int GetUserId(string userName)
        {
            return _ctx.Users.Where(u => u.Email == userName).Select(u => u.Id).FirstOrDefault();
        }

        public string GetUserName(int id)
        {
            return _ctx.Users.Where(u => u.Id == id).Select(u => u.Email).FirstOrDefault();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _ctx.Users.Where(u => u.Id == id).Select(u => u).FirstOrDefaultAsync();
        }

        public async Task RegisterLoginAttemptToDatabaseAsync(string email, string ipAddress, bool isSuccess)
        {
            var userId = GetUserId(email);
            var user = GetUserByIdAsync(userId);
            if (user == null) throw new ArgumentNullException(nameof(user));

            _ctx.Attempts.Add(new LoginAttempt()
            {
                User = await user,
                IpAddress = ipAddress,
                IsSuccess = isSuccess,
                TimeStamp = DateTime.Now
            });

            await _ctx.SaveChangesAsync();
        }
    }
}
