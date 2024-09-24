
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;
namespace PowerliftingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonalRecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ApiResponse _response;

    public PersonalRecordsController(ApplicationDbContext context)
    {
        _context = context;
        _response = new ApiResponse();
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse>> GetPersonalRecordsByUserId(string userId)
    {
        var records = await _context.PersonalRecords
            .Where(r => r.UserId == userId)
            .ToListAsync();
    
        _response.Result = records;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreatePersonalRecord([FromBody] PersonalRecordCreateDTO personalRecordCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "Model is not valid" };
            return BadRequest(_response);
        }

        PersonalRecord record = new PersonalRecord()
        {
            UserId = personalRecordCreateDto.UserId,
            ExerciseId = personalRecordCreateDto.ExerciseId,
            OneRepMax = personalRecordCreateDto.OneRepMax,
            Date = DateTime.Now
        };

        _context.PersonalRecords.Add(record);
        await _context.SaveChangesAsync();

        _response.Result = record;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

}