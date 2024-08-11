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
        public DbSet<CustomExercises> CustomExercises { get; set; }
        public DbSet<WorkoutExercises> ExercisesInWorkout { get; set; }
        public DbSet<Sets> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);

            // Seed data for Exercises
            builder.Entity<CustomExercises>().HasData(
                new CustomExercises { Id = 1, Name = "Exercise 1", Description = "Description for Exercise 1",  UserId = null },
                new CustomExercises { Id = 2, Name = "Exercise 2", Description = "Description for Exercise 2", UserId = null },
                new CustomExercises { Id = 3, Name = "Exercise 3", Description = "Description for Exercise 3",  UserId = null }
            );
        }
    }
}