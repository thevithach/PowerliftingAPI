using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

public interface IWorkoutRepository
{
    Task<IEnumerable<WorkoutsDTO>> GetAllWorkoutsAsync();
    Task<Workouts?> GetWorkoutById(int id);
    Task<Workouts?> GetActiveWorkoutById(string userId);
    Task<IEnumerable<object>> GetNonActiveWorkouts(string userId);
    Task<bool> HasActiveWorkout(string userId);
    Task<Workouts> CreateWorkout(WorkoutCreateDTO workoutCreateDto);
    Task DeleteWorkoutById(int id);
    Task<Workouts?> EndActiveWorkout(EndActiveWorkoutDTO dto);
    Task<Workouts> SaveWorkout(SaveWorkoutDTO dto);
    Task<bool> WorkoutExists(int id);
}