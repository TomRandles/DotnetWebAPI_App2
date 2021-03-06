using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Models
{
    public class ProgrammeDto
    {
        [Required(ErrorMessage = "Programme ID is required.")] 
        public string ProgrammeId { get; set; }

        [Required(ErrorMessage = "Programme name is required.")]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
    }
}