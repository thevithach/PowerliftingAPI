using Microsoft.AspNetCore.Mvc;
using PowerliftingAPI.Data;
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
    
    
}