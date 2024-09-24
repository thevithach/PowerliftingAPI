namespace PowerliftingAPI.Dto;

public class PersonalRecordCreateDTO
{
    public string UserId { get; set; }
    public int ExerciseId { get; set; }
    public double OneRepMax { get; set; }
    public DateTime Date { get; set; } 
}