using AutoMapper;

namespace School.Domain.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Entities.Student, Models.StudentDto>();
            CreateMap<Models.StudentDto, Entities.Student>();
        }
    }
}
