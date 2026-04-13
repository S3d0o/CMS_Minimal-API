using AutoMapper;
using CMS.Application.DTOs.StudentDtos;
using CMS.Domain.Entities;

namespace CMS.Application.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentCreateOrUpdateDto, Student>();
            CreateMap<Student, StudentReadDto>();
        }
    }
}
