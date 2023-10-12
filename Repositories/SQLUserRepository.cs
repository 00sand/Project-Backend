using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly TaskDbContext dbContext;

        public SQLUserRepository(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (existingUser == null)
            {
                return null;
            }

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> UpdateAsync(Guid id, User user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (existingUser == null)
            {
                return null;
            }

            if (user.Email != null)
                existingUser.Email = user.Email;

            if (user.Password != null)
                existingUser.Password = user.Password;

            if (user.UserName != null)
                existingUser.UserName = user.UserName;

            await dbContext.SaveChangesAsync();
            return existingUser;
        }

    }
}
