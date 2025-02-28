using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD4.Data
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Assignment")] 
        public int AssignmentId { get; set; }

        [ForeignKey("Student")] 
        public int StudentId { get; set; }

        public DateTime SubmissionTime { get; set; }
        public decimal Score { get; set; }

        // navigacijas saitess
        public Assignment Assignment { get; set; } // šis ļauj ieladet assignment diiscription
        public Student Student { get; set; } // šis ļauj ieladet student fullname
    }
}
