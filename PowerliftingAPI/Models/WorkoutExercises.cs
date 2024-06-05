namespace PowerliftingAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkoutExercises
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Workout")]
    public int WorkoutId { get; set; }
    public virtual Workouts Workout { get; set; }

    [ForeignKey("Exercise")]
    public int ExerciseId { get; set; }
    public virtual Exercises Exercise { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; }
}

