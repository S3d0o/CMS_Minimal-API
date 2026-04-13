using AutoMapper;
using CMS.Application.DTOs.CourseDTOs;
using CMS.Domain.Entities;

namespace CMS.Application.MappingProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseReadDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<CourseUpdateDto, Course>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
