using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerliftingAPI.Models;

public class Workouts
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }
    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    
    public virtual ICollection<WorkoutExercises> WorkoutExercises { get; set; }
    
    public bool isActive { get; set; }
}
