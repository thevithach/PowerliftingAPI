using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomExercisesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;
    
    public CustomExercisesController(ApplicationDbContext context)
    {
       _context = context;
       _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        _response.Result = await _context.CustomExercises.ToListAsync();
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

        var exercise = await _context.CustomExercises.FirstOrDefaultAsync(u => u.Id == id);
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
    public async Task<ActionResult<ApiResponse>> CreateExercise([FromBody] CustomExerciseCreateDTO customExerciseCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "Invalid Model" };
            return BadRequest(_response);
        }

        CustomExercises exerciseToCreate = new CustomExercises()
        {
            Name = customExerciseCreateDto.Name,
            Description = customExerciseCreateDto.Description,
            UserId = customExerciseCreateDto.UserId,
        };
        
        // Check if the user exists only if UserId is not null
        if (exerciseToCreate.UserId != null)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == exerciseToCreate.UserId);
            if (!userExists)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorsMessages = new List<string>() { "User does not exist" };
                return BadRequest(_response);
            }
        }
        
        _context.CustomExercises.Add(exerciseToCreate);
        await _context.SaveChangesAsync();

        _response.Result = customExerciseCreateDto;
        _response.StatusCode = HttpStatusCode.OK;
        return CreatedAtAction(nameof(CreateExercise), new { id = exerciseToCreate.Id }, _response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateExerciseById(int id,
        [FromBody] CustomExerciseUpdateDTO customExerciseUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
        }

        if (customExerciseUpdateDto == null || id != customExerciseUpdateDto.Id)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }

        var exerciseToUpdate = await _context.CustomExercises.FirstOrDefaultAsync(u => u.Id == id);

        if (exerciseToUpdate == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }

        exerciseToUpdate.Name = customExerciseUpdateDto.Name;
        exerciseToUpdate.Description = customExerciseUpdateDto.Description;
        exerciseToUpdate.UserId = customExerciseUpdateDto.UserId;

        _context.CustomExercises.Update(exerciseToUpdate);
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

        var exerciseDoesExist = await _context.CustomExercises.AnyAsync(u => u.Id == id);
        if (!exerciseDoesExist)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Exercise was not found" };
            return NotFound(_response);
        }
        
        var exerciseToBeDeleted = await _context.CustomExercises.FirstOrDefaultAsync(u => u.Id == id);
        
        if (exerciseToBeDeleted == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Record is empty" };
            return BadRequest();
        }
        
        _context.CustomExercises.Remove(exerciseToBeDeleted);
        await _context.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
    
    
    
}