using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WorkoutExerciseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;
    
    public WorkoutExerciseController(ApplicationDbContext context)
    {
        _context = context;
        _response = new ApiResponse();
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetWorkoutExercises()
    {
        _response.Result = await _context.ExercisesInWorkout.ToListAsync();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }
    
    [HttpGet("{workoutId}")]
    public async Task<ActionResult<ApiResponse>> GetExercisesByWorkoutId(int workoutId)
    {
        var exercises = await _context.ExercisesInWorkout
            .Where(e => e.WorkoutId == workoutId)
            .ToListAsync();

        if (exercises == null || !exercises.Any())
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string> { "No exercises found for this workout" };
            return NotFound(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = exercises;

        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddExerciseToWorkout(WorkoutExerciseAddDTO workoutExerciseAddDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = ["Model is not valid"];
            return BadRequest(_response);
        }
        
        if (workoutExerciseAddDto.ExercisesId is null or 0 &&
            workoutExerciseAddDto.CustomExerciseId is null or 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string> { "Both ExercisesId and CustomExercisesId cannot be null or zero." };
            return BadRequest(_response);
        }
        
        // Check if the CustomExercise exists
        if (workoutExerciseAddDto.CustomExerciseId.HasValue)
        {
            var customExerciseExists = await _context.CustomExercises.AnyAsync(ce => ce.Id == workoutExerciseAddDto.CustomExerciseId.Value);
            if (!customExerciseExists)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorsMessages = ["CustomExercise does not exist"];
                return BadRequest(_response);
            }
        }

        WorkoutExercises workoutExercises = new WorkoutExercises()
        {
            WorkoutId = workoutExerciseAddDto.WorkoutId,
            CustomExercisesId = workoutExerciseAddDto.CustomExerciseId,
            ExercisesId = workoutExerciseAddDto.ExercisesId,
            Sets = workoutExerciseAddDto.Sets.Select((set, index) => new Sets
            {
                SetNumber = index + 1,
                Repetitions = set.Repetitions,
                Weight = set.Weight
            }).ToList()
        };

        await _context.ExercisesInWorkout.AddAsync(workoutExercises);

        // Calculate and update PRs
        foreach (var set in workoutExercises.Sets)
        {
            var oneRepMax = set.Weight + (set.Weight * set.Repetitions * (decimal)0.0333);
            if (workoutExerciseAddDto.ExercisesId.HasValue)
            {
                var personalRecord = await _context.PersonalRecords
                    .Where(pr => pr.UserId == workoutExerciseAddDto.UserId && pr.ExerciseId == workoutExerciseAddDto.ExercisesId.Value)
                    .FirstOrDefaultAsync();

                if (personalRecord == null)
                {
                    // Create a new personal record
                    personalRecord = new PersonalRecord
                    {
                        UserId = workoutExerciseAddDto.UserId,
                        ExerciseId = workoutExerciseAddDto.ExercisesId.Value,
                        OneRepMax = (double)oneRepMax, // Convert to double
                        Date = DateTime.Now
                    };
                    _context.PersonalRecords.Add(personalRecord);
                }
                else if ((double)oneRepMax > personalRecord.OneRepMax)
                {
                    // Update the existing personal record
                    personalRecord.OneRepMax = (double)oneRepMax;
                    personalRecord.Date = DateTime.Now;
                    _context.PersonalRecords.Update(personalRecord);
                }
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }

        await _context.SaveChangesAsync();


        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = workoutExerciseAddDto;
        return Ok(_response);
    }
    
    
}