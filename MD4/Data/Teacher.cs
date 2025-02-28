using System.ComponentModel.DataAnnotations;

namespace MD4.Data
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime ContractDate { get; set; }

        public string FullName => $"{Name} {Surname}";
    }
}
