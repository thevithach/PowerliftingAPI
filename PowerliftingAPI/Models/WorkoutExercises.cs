
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

    [ForeignKey("ExercisesId")]
    public int? ExercisesId { get; set; }
    public virtual Exercises Exercises { get; set; }
    [ForeignKey("CustomExercisesId")]
    public int? CustomExercisesId { get; set; }
    public virtual CustomExercises CustomExercises { get; set; }
    
    public virtual ICollection<Sets> Sets { get; set; }

}

