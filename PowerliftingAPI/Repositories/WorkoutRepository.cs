using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly ApplicationDbContext _context;

    public WorkoutRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutsDTO>> GetAllWorkouts()
    {
        return await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Sets)
            .Select(w => new WorkoutsDTO
            {
                Id = w.Id,
                Title = w.Title,
                Date = w.Date,
                Notes = w.Notes,
                UserId = w.UserId,
                isActive = w.isActive,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExercisesDTO
                {
                    Id = we.Id,
                    WorkoutId = we.WorkoutId,
                    ExercisesId = we.ExercisesId,
                    CustomExercisesId = we.CustomExercisesId,
                    Sets = we.Sets.Select(s => new SetsDTO
                    {
                        Id = s.Id,
                        WorkoutExerciseId = s.WorkoutExerciseId,
                        SetNumber = s.SetNumber,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();
    }
}