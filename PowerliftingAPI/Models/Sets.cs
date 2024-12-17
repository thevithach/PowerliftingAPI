using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace PowerliftingAPI.Models;

    public class Sets
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("WorkoutExerciseId")]
        public int WorkoutExerciseId { get; set; }
        [JsonIgnore] // Prevent serialization of WorkoutExercise reference
        public virtual WorkoutExercises WorkoutExercise { get; set; }

        [Required]
        public int SetNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Repetitions { get; set; }

        [Required]
        public decimal Weight { get; set; }
    }