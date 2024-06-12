namespace PowerliftingAPI.Dto;

public class UserDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public List<CustomExerciseGetDTO> CustomExercises { get; set; }
}