namespace PowerliftingAPI.Models;

public class PersonalRecord
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ExerciseId { get; set; }
    public double OneRepMax { get; set; }
    public DateTime Date { get; set; } 

    public ApplicationUser User { get; set; }
    public Exercises Exercise { get; set; }
}