namespace PowerliftingAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Exercises
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public bool IsCustom { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }
    
    public virtual ApplicationUser? User { get; set; }
}