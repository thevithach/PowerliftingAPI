namespace PowerliftingAPI.Dto;

public class WorkoutsDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public string UserId { get; set; }
    public bool isActive { get; set; }
    public ICollection<WorkoutExercisesDTO> WorkoutExercises { get; set; }
}