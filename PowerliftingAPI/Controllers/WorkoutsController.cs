using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
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
}