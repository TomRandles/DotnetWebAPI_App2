using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Models
{
    public class StudentDto
    {

        [Required(ErrorMessage = "Student ID is required.")] 
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(200)] 
        public string Description { get; set; }

        [Required]
        public string ProgrammeId { get; set; }
    }
}