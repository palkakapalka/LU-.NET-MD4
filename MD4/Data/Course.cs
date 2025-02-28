using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MD4.Data
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }
        [Required]
       
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }

}
