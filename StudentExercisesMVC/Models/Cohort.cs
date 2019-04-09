using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
    public class Cohort
    {
        public int Id { get; set; }
        [StringLength(11, MinimumLength = 5)]
        public string cohortName { get; set; }
        public List<Student> studentsInCohort { get; set; } = new List<Student>();
        public List<Instructor> instructorList { get; set; } = new List<Instructor>();
    }
}
