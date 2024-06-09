using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WorkoutsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;
    public WorkoutsController(ApplicationDbContext context)
    {
        _context = context;
        _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWorkouts()
    {
        var workouts = await _context.Workouts.Select(e => new 
        {
            e.Id, e.Title, e.Date, e.Notes, e.UserId, e.WorkoutExercises
        }).ToListAsync();

        if (workouts.Count == 0)
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

        Workouts workouts = new Workouts()
        {
            Title = workoutCreateDto.Title,
            Date = workoutCreateDto.Date,
            Notes = workoutCreateDto.Notes,
            UserId = workoutCreateDto.UserId
        };

        _context.Workouts.Add(workouts);
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = workoutCreateDto;

        return Ok(_response);
    }
}