using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        _secretKey = configuration.GetSection("jwtKeyValue").Value!;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users
            .Include(u => u.CustomExercises)
            .ToListAsync();

        if (users.Count == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorsMessages = new List<string>() { "No users found" };
            return BadRequest(_response);
        }

        // Map ApplicationUser entities to UserDTOs
        var userDTOs = users.Select(u => new UserDTO
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            City = u.City,
            CustomExercises = u.CustomExercises.Select(ce => new CustomExerciseGetDTO()
            {
                Id = ce.Id,
                Name = ce.Name,
            }).ToList()
        }).ToList();

        _response.Result = userDTOs;
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

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
    {
        var userFromDb =
            await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(loginRequestDto.Email.ToLower()));

        bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password);

        if (isValid == false)
        {
            _response.Result = new LoginRequestDTO();
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages.Add("Invalid username or password");
            return BadRequest(_response);
        }
        
        // Token generation
        var roles = await _userManager.GetRolesAsync(userFromDb);
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_secretKey);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                // Can add more claims later..
                new Claim("id", userFromDb.Id.ToString()),
                new Claim("lastName", userFromDb.LastName),
                new Claim(ClaimTypes.Email, userFromDb.Email.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponseDTO loginResponse = new()
        {
            Email = userFromDb.Email,
            Token = tokenHandler.WriteToken(token)
        };

        if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorsMessages.Add("Invalid username or password");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = loginResponse;
        return Ok(_response);

    }
    


}