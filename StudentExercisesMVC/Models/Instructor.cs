using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
    public class Instructor
    {
        public int id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 3)]
        public string slackHandle { get; set; }
        public string CohortName { get; set; }
        public int CohortId { get; set; }

        public List<Cohort> Cohorts { get; set; } = new List<Cohort>();
    }
}
