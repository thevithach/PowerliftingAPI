using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PowerliftingAPI.Models;

public class ApplicationUser : IdentityUser
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
}