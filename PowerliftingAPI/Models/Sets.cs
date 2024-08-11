using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerliftingAPI.Models;

    public class Sets
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("WorkoutExerciseId")]
        public int WorkoutExerciseId { get; set; }

        public virtual WorkoutExercises WorkoutExercise { get; set; }

        [Required]
        public int SetNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Repetitions { get; set; }

        [Required]
        public decimal Weight { get; set; }
    }