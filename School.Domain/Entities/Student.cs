using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Entities
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage ="Student ID is required.")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(200)] 
        public string Description { get; set; }

        public Programme Programme { get; set; }

        [ForeignKey("Programmes")]
        [Required(ErrorMessage = "Programme ID FK is required.")]
        public string ProgrammeId { get; set; }
    }
}