using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace TaskManagerAPI.Data
{
    public class TaskManagerAuthDbContext : IdentityDbContext

    {
        public TaskManagerAuthDbContext(DbContextOptions<TaskManagerAuthDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "a58c4afe-1fa5-4e20-bf54-c4fba04d36ad";
            var userRoleId = "4e4bfc79-da95-43ab-8a0f-5f4979a7a53d";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },

                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }

            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
