using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    public readonly ApplicationDbContext _context;
    private ApiResponse _response;
    public ExerciseController(ApplicationDbContext context)
    {
        _context = context;
        _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        /*
         var exercises = await _context.Exercises
           .Select(e => new 
           {
               e.Id,
               e.Name,
               e.Description
           })
           .ToListAsync();
         */
        var exerciseList = await _context.Exercises.ToListAsync();
        if (exerciseList.Count == 0)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorsMessages = new List<string>() { "No exercises found" };
            return NotFound(_response);
        }

        _response.Result = exerciseList;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateExercise([FromBody] ExerciseCreateDTO exerciseCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
        }

        Exercises exercises = new Exercises()
        {
            Name = exerciseCreateDto.Name,
            Description = exerciseCreateDto.Description,
        };

        _context.Exercises.Add(exercises);
        await _context.SaveChangesAsync();

        _response.Result = exerciseCreateDto;
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateExerciseById(int id, [FromBody] ExerciseUpdateDTO exerciseUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
        }

        if (id == 0)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            _response.ErrorsMessages = new List<string>() { "No exercise at id 0" };
        }

        if (id != exerciseUpdateDto.Id)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }

        var exerciseToBeUpdated = await _context.Exercises.FirstOrDefaultAsync(u => u.Id == id);

        if (exerciseToBeUpdated == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
        }


        exerciseToBeUpdated.Name = exerciseUpdateDto.Name;
        exerciseToBeUpdated.Description = exerciseUpdateDto.Description;
 

        _context.Exercises.Update(exerciseToBeUpdated);
        await _context.SaveChangesAsync();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = exerciseUpdateDto;

        return Ok(_response);

    }
}