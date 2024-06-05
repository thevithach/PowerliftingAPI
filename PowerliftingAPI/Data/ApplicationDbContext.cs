using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Workouts> Workouts { get; set; }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<WorkoutExercises> WorkoutExercises { get; set; }
        public DbSet<ExerciseLog> ExerciseLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);

            // Seed data for Exercises
            builder.Entity<Exercises>().HasData(
                new Exercises { Id = 1, Name = "Exercise 1", Description = "Description for Exercise 1", IsCustom = false, UserId = null },
                new Exercises { Id = 2, Name = "Exercise 2", Description = "Description for Exercise 2", IsCustom = true, UserId = null },
                new Exercises { Id = 3, Name = "Exercise 3", Description = "Description for Exercise 3", IsCustom = false, UserId = null }
            );
        }
    }
}