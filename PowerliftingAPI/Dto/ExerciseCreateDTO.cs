using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Dto;

public class ExerciseCreateDTO
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
}