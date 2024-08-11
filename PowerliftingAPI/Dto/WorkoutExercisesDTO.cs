namespace PowerliftingAPI.Dto;

public class WorkoutExercisesDTO
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int? ExercisesId { get; set; }
    public int? CustomExercisesId { get; set; }
    public ICollection<SetsDTO> Sets { get; set; }
}

public class SetsDTO
{
    public int Id { get; set; }
    public int WorkoutExerciseId { get; set; }
    public int SetNumber { get; set; }
    public int Repetitions { get; set; }
    public decimal Weight { get; set; }
}