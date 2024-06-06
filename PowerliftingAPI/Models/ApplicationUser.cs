using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PowerliftingAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    [ValidateNever]
    public virtual ICollection<Workouts> Workouts { get; set; }
    [ValidateNever]
    public virtual ICollection<Exercises> CustomExercises { get; set; }
}