public class WorkoutExerciseAddDTO
{
    public int WorkoutId { get; set; }
    public int? CustomExerciseId { get; set; }
    public int? ExercisesId { get; set; }
    public string? UserId { get; set; }
    public List<SetDTO> Sets { get; set; }
}

public class SetDTO
{
    public int Repetitions { get; set; }
    public decimal Weight { get; set; }
}