using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Models
{
    public class StudentDto
    {
        [Key]
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProgrammeDto Programme { get; set; }

        [ForeignKey("Programmes")]
        public string ProgrammeId { get; set; }
    }
}