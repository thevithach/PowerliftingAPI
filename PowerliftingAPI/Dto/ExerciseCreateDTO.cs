using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Dto;

public class ExerciseCreateDTO
{


    [Required]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public bool IsCustom { get; set; }

    public string? UserId { get; set; }
    
}
