using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD4.Data
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [ForeignKey("Curse")]
        public int CourseId { get; set; } // Foreign key to Course
        [Required]
        public string Description { get; set; }

        public Course Course { get; set; } 
    }
}
