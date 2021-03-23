using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Programme
    {
        [Key]
        [Required(ErrorMessage = "Programme ID is required.")]
        public string ProgrammeId { get; set; }

        [Required(ErrorMessage ="Name is required.")]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}