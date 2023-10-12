using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> dbContextOptions) : base(dbContextOptions) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<StatusCategory> StatusCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>().HasKey(t => t.TaskId);
            modelBuilder.Entity<TaskModel>()
            .HasOne(t => t.StatusCategory)
            .WithMany()
            .HasForeignKey(t => t.StatusCategoryId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne()
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<StatusCategory>().HasData(
            new StatusCategory { StatusCategoryId = 1, StatusName = "Assigned" },
            new StatusCategory { StatusCategoryId = 2, StatusName = "In Progress" },
            new StatusCategory { StatusCategoryId = 3, StatusName = "Completed" }
            );

            modelBuilder.Entity<TaskModel>()
           .Property(t => t.StatusCategoryId)
           .HasDefaultValue(1);

        }

       
    }
}
