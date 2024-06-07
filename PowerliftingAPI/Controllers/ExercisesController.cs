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
    public async Task<IActionResult> GetAllExercises()
    {
        _response.Result = await _context.Exercises.ToListAsync();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExerciseById(int id)
    {
        if (id == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "Invalid ID" };
            return BadRequest(_response);
        }

        var exercise = await _context.Exercises.FirstOrDefaultAsync(u => u.Id == id);
        if (exercise == null)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            return NotFound(_response);
        }

        _response.Result = exercise;
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateExerciseById(int id,
        [FromBody] ExerciseUpdateDTO exerciseUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
        }

        if (exerciseUpdateDto == null || id != exerciseUpdateDto.Id)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }

        var exerciseToUpdate = await _context.Exercises.FirstOrDefaultAsync(u => u.Id == id);

        if (exerciseToUpdate == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }

        exerciseToUpdate.Name = exerciseUpdateDto.Name;
        exerciseToUpdate.Description = exerciseUpdateDto.Description;
        exerciseToUpdate.IsCustom = exerciseUpdateDto.IsCustom;
        exerciseToUpdate.UserId = exerciseUpdateDto.UserId;

        _context.Exercises.Update(exerciseToUpdate);
        await _context.SaveChangesAsync();
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse>> DeleteExerciseById(int id)
    {
        if (id == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "No object found at id 0" };
            return BadRequest(_response);
        }

        var exerciseDoesExist = await _context.Exercises.AnyAsync(u => u.Id == id);
        if (!exerciseDoesExist)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Exercise was not found" };
            return NotFound(_response);
        }
        
        var exerciseToBeDeleted = await _context.Exercises.FirstOrDefaultAsync(u => u.Id == id);
        
        if (exerciseToBeDeleted == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Record is empty" };
            return BadRequest();
        }
        
        _context.Exercises.Remove(exerciseToBeDeleted);
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
    
    
    
}