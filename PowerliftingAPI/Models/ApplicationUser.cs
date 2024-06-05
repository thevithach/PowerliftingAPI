using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PowerliftingAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public virtual ICollection<Workouts> Workouts { get; set; }
    public virtual ICollection<Exercises> CustomExercises { get; set; }
}