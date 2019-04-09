using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 3)]
        public string slackHandle { get; set; }

        public string cohortName { get; set; }
        public int cohortId { get; set; }


        public Cohort cohort { get; set; } = new Cohort();
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
