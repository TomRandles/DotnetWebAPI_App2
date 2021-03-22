using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Models
{
    public class StudentDto
    {
        [Key]
        [Required(ErrorMessage ="Student ID is required.")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(200)] 
        public string Description { get; set; }
        public ProgrammeDto Programme { get; set; }

        [ForeignKey("Programmes")]
        [Required(ErrorMessage = "Programme ID FK is required.")]
        public string ProgrammeId { get; set; }
    }
}