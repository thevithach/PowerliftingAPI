using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Models;

public class Exercises
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
    public ICollection<WorkoutExercises> WorkoutExercises { get; set; }
}