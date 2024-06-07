using System.ComponentModel.DataAnnotations;

namespace PowerliftingAPI.Dto;

public class CustomExerciseCreateDTO
{


    [Required]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
    
    public string? UserId { get; set; }
    
}