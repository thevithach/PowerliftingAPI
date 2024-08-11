using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<WorkoutsDTO>> GetAllWorkouts();

}