using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Models
{
    public class ProgrammeDto
    {
        [Key]
        public string ProgrammeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<StudentDto> Students { get; set; }
    }
}