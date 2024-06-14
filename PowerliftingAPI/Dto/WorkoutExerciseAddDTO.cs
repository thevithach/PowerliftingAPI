using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Dto;

public class WorkoutExerciseAddDTO
{
    [Required]
    public int WorkoutId { get; set; }

    public int? ExerciseId { get; set; }
    public int? CustomExerciseId { get; set; }
    
    [Required]
    public int Repetitions { get; set; }
    [Required]
    public decimal Weight { get; set; }
}