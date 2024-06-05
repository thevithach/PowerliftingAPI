using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerliftingAPI.Models;

public class Workouts
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<WorkoutExercises> WorkoutExercises { get; set; }
}
