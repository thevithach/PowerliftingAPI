namespace PowerliftingAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkoutExercises
{
    [Key]
    public int Id { get; set; }
    [Required]
    [ForeignKey("WorkoutId")]
    public int WorkoutId { get; set; }
    public virtual Workouts Workout { get; set; }

    [ForeignKey("ExerciseId")]
    public int? ExerciseId { get; set; }
    public virtual Exercises Exercises { get; set; }
    [ForeignKey("CustomExerciseId")]
    public int? CustomExerciseId { get; set; }
    public CustomExercises CustomExercises { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Repetitions { get; set; }
    [Required]
    public decimal Weight { get; set; }

}

