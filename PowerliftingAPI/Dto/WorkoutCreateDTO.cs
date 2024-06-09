using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Dto;

public class WorkoutCreateDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }
    [Required]
    public string UserId { get; set; }
}