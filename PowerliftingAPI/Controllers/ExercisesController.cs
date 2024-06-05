using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;
    
    public ExercisesController(ApplicationDbContext context)
    {
       _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetExercises()
    {
        _response.Result = await _context.Exercises.ToListAsync();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);

    }
    
    
}