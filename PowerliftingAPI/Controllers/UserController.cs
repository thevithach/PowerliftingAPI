using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerliftingAPI.Data;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;
using PowerliftingAPI.Utility;

namespace PowerliftingAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    public readonly ApplicationDbContext _context;
    private ApiResponse _response;
    private string _secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _response = new ApiResponse();
        _secretKey = configuration.GetValue<string>("JWT:Secret");
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.Select(e => new
            {
                e.Id,
                e.UserName,
                e.FirstName,
                e.LastName,
                e.PhoneNumber,
                e.Address,
                e.City,
            }).
            ToListAsync();
        if (users.Count == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "No users found" };
            return BadRequest(_response);
        }

        _response.Result = users;
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDto)
    {
        var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == registerRequestDto.Email.ToLower());

        if (userFromDb != null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages.Add("Username already exists");
            return BadRequest(_response);
        }

        ApplicationUser user = new ApplicationUser()
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.Email,
            NormalizedEmail = registerRequestDto.Email.ToUpper(),
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
            Address = registerRequestDto.Address,
            City = registerRequestDto.City,
            PhoneNumber = registerRequestDto.PhoneNumber,
            CustomExercises = null,
            Workouts = null
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    //Create roles in database
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                }

                if (registerRequestDto.Role.ToLower() == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        _response.ErrorsMessages.Add("Error while registering");
        return BadRequest(_response);
    }


}