namespace PowerliftingAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExerciseLog
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("WorkoutExercise")]
    public int WorkoutExerciseId { get; set; }
    public virtual WorkoutExercises WorkoutExercise { get; set; }

    [Required]
    public int SetNumber { get; set; }

    [Required]
    public int Repetitions { get; set; }

    [Required]
    public double Weight { get; set; }
}

