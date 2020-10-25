using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<SchoolType> SchoolTypes { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ApplicationUserTask> ApplicationUserTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserTask>()
            .HasKey(at => new { at.ApplicationUserId, at.TaskId });

            modelBuilder.Entity<ApplicationUserTask>()
                .HasOne(at => at.ApplicationUser)
                .WithMany(t => t.ApplicationUserTasks)
                .HasForeignKey(pt => pt.ApplicationUserId);

            modelBuilder.Entity<ApplicationUserTask>()
                .HasOne(at => at.Task)
                .WithMany(t => t.ApplicationUserTasks)
                .HasForeignKey(at => at.TaskId);
        }
    }
}
