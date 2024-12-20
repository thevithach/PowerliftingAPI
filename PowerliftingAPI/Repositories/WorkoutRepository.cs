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

    public async Task<IEnumerable<WorkoutsDTO>> GetAllWorkoutsAsync()
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

    public async Task<Workouts?> GetWorkoutById(int id)
    {
        return await _context.Workouts.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Workouts?> GetActiveWorkoutById(string userId)
    {
        return await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Sets)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.isActive);
    }


    public async Task<IEnumerable<object>> GetNonActiveWorkouts(string userId)
    {
        return await _context.Workouts
            .Where(w => w.UserId == userId && !w.isActive)
            .Select(w => new
            {
                w.Id,
                w.Title,
                w.Date,
                w.Notes,
                w.UserId,
                WorkoutExercises = w.WorkoutExercises.Select(we => new
                {
                    we.Id,
                    we.WorkoutId,
                    we.ExercisesId,
                    we.CustomExercisesId,
                    Sets = we.Sets.Select(s => new
                    {
                        s.Id,
                        s.WorkoutExerciseId,
                        s.SetNumber,
                        s.Repetitions,
                        s.Weight
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<bool> HasActiveWorkout(string userId)
    {
        return await _context.Workouts.AnyAsync(u => u.UserId == userId && u.isActive);
    }

    public async Task<Workouts> CreateWorkout(WorkoutCreateDTO workoutCreateDto)
    {
        var workout = new Workouts
        {
            Title = workoutCreateDto.Title,
            Date = workoutCreateDto.Date,
            Notes = workoutCreateDto.Notes,
            UserId = workoutCreateDto.UserId,
            isActive = workoutCreateDto.isActive
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        return workout;
    }

    public async Task DeleteWorkoutById(int id)
    {
        var workoutFromDb = await _context.Workouts.FirstOrDefaultAsync(u => u.Id == id);

        if (workoutFromDb != null)
        {
            _context.Workouts.Remove(workoutFromDb);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Workouts?> EndActiveWorkout(EndActiveWorkoutDTO dto)
    {
        var activeWorkout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.UserId == dto.UserId && w.isActive);

        if (activeWorkout == null)
            return null;

        activeWorkout.Title = string.IsNullOrWhiteSpace(dto.FinalTitle)
            ? activeWorkout.Title
            : dto.FinalTitle;
        
        activeWorkout.isActive = false;

        await _context.SaveChangesAsync();
        return activeWorkout;
    }

    public async Task<Workouts> SaveWorkout(SaveWorkoutDTO dto)
    {
        var activeWorkout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.UserId == dto.UserId && w.isActive);

        if (activeWorkout == null)
            return null;

        activeWorkout.Title = string.IsNullOrWhiteSpace(dto.Title)
            ? activeWorkout.Title
            : dto.Title;
        activeWorkout.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return activeWorkout;
    }

    public async Task<bool> WorkoutExists(int id)
    {
        return await _context.Workouts.AnyAsync(u => u.Id == id);
    }
}