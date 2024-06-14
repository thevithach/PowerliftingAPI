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
            ExercisesId = workoutExerciseAddDto.ExerciseId,
            Weight = workoutExerciseAddDto.Weight,
            Repetitions = workoutExerciseAddDto.Repetitions
        };

        await _context.ExercisesInWorkout.AddAsync(workoutExercises);
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = workoutExerciseAddDto;
        return Ok(_response);
    }
}