using AutoMapper;

namespace School.Domain.Profiles
{
    public class ProgrammeProfile : Profile
    {
        public ProgrammeProfile()
        {
            CreateMap<Entities.Programme, Models.ProgrammeDto>();
            CreateMap<Models.ProgrammeDto, Entities.Programme>();
        }
    }
}