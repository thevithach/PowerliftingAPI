using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;
using PowerliftingAPI.Repositories;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WorkoutsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutsController(ApplicationDbContext context, IWorkoutRepository workoutRepository)
    {
        _context = context;
        _response = new ApiResponse();
        _workoutRepository = workoutRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWorkouts()
    {
        var workouts = await _workoutRepository.GetAllWorkouts();

        if (!workouts.Any())
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "There are no workouts" };
            return BadRequest(_response);
        }

        _response.Result = workouts;
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkoutById(int id)
    {
        if (id == 0)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Nothing associated with Id 0" };
            return NotFound(_response);
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(u => u.Id == id);

        if (workout == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Nothing associated with Id 0" };
            return BadRequest(_response);
        }

        _response.Result = workout;
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
    
    /*[HttpGet("{userId}")]
    public async Task<IActionResult> GetWorkoutByUserId(string userId)
    {
        var workout = await _context.Workouts.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
        if (workout == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "No workout found for this user" };
            return BadRequest(_response);
        }

        _response.Result = workout;
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }*/
    
    [HttpGet("active/{userId}")]
    public async Task<ActionResult<ApiResponse>> GetActiveWorkout(string userId)
    {
        var activeWorkout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Sets)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.isActive);

        if (activeWorkout == null)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string> { "No active workout found for the user" };
            return NotFound(_response);
        }

        // Create a new WorkoutsDTO and map the properties
        var activeWorkoutDto = new WorkoutsDTO
        {
            Id = activeWorkout.Id,
            Title = activeWorkout.Title,
            Date = activeWorkout.Date,
            Notes = activeWorkout.Notes,
            UserId = activeWorkout.UserId,
            isActive = activeWorkout.isActive,
            WorkoutExercises = activeWorkout.WorkoutExercises.Select(we => new WorkoutExercisesDTO
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
        };

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = activeWorkoutDto;

        return Ok(_response);
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse>> GetNonActiveWorkouts(string userId)
    {
        var nonActiveWorkouts = await _context.Workouts
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

        if (nonActiveWorkouts == null || !nonActiveWorkouts.Any())
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string> { "No non-active workouts found for the user" };
            return NotFound(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = nonActiveWorkouts;

        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateWorkout([FromBody] WorkoutCreateDTO workoutCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
            return BadRequest(_response);
        }

        // Check if there's an active workout only if the new workout is meant to be active
        if (workoutCreateDto.isActive)
        {
            var activeWorkout = await _context.Workouts.AnyAsync(u => u.UserId == workoutCreateDto.UserId && u.isActive);
            if (activeWorkout)
            {
                _response.ErrorsMessages = new List<string> { "A workout is already active, finish your current one before creating a new active workout" };
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        Workouts workout = new Workouts()
        {
            Title = workoutCreateDto.Title,
            Date = workoutCreateDto.Date,
            Notes = workoutCreateDto.Notes,
            UserId = workoutCreateDto.UserId,
            isActive = workoutCreateDto.isActive
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.Created;
        _response.IsSuccess = true;
        _response.Result = workoutCreateDto;

        return Ok(_response);

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkoutById(int id)
    {

        var workoutDoesExist = await _context.Workouts.AnyAsync(u => u.Id == id);
        if (!workoutDoesExist)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "The workout does not exist" };
            return NotFound(_response);
        }
        
        var workoutFromDb = await _context.Workouts.FirstOrDefaultAsync(u => u.Id == id);

        if (workoutFromDb == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "The record is empty (workout)" };
            return BadRequest(_response);
        }

        _context.Workouts.Remove(workoutFromDb);
        await _context.SaveChangesAsync();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);

    }
    
    [HttpPut("endActive")]
    public async Task<ActionResult<ApiResponse>> EndActiveWorkout([FromBody] EndActiveWorkoutDTO dto)
    {
        var activeWorkout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.UserId == dto.UserId && w.isActive);

        if (activeWorkout == null)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string> { "No active workout found for the user" };
            return NotFound(_response);
        }

        // Update the title if provided, otherwise keep existing
        activeWorkout.Title = string.IsNullOrWhiteSpace(dto.FinalTitle) ? activeWorkout.Title : dto.FinalTitle;
        activeWorkout.isActive = false;
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = activeWorkout;

        return Ok(_response);
    }
    
    
}