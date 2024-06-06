using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
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
       _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetExercises()
    {
        _response.Result = await _context.Exercises.ToListAsync();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateExercise([FromBody] ExerciseCreateDTO exerciseCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "Invalid Model" };
            return BadRequest(_response);
        }

        Exercises exerciseToCreate = new Exercises()
        {
            Name = exerciseCreateDto.Name,
            Description = exerciseCreateDto.Description,
            IsCustom = exerciseCreateDto.IsCustom,
            UserId = exerciseCreateDto.UserId,
        };
        
        // Check if the user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == exerciseToCreate.UserId);
        if (!userExists)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "User does not exist" };
            return BadRequest(_response);
        }
        
        
        
        _context.Exercises.Add(exerciseToCreate);
        await _context.SaveChangesAsync();

        _response.Result = exerciseCreateDto;
        _response.StatusCode = HttpStatusCode.OK;
        return CreatedAtAction(nameof(CreateExercise), new { id = exerciseToCreate.Id }, _response);
    }
    
}