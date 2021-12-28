using AutoMapper;
using DataAccess.Entities;
using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperConfiguration
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentEntity, StudentDto>().ReverseMap();
            CreateMap<GroupEntity, GroupDto>().ReverseMap();
            CreateMap<SubjectEntity, SubjectDto>().ReverseMap();
            CreateMap<ScheduledLesson, ScheduledLessonDto>().ReverseMap();
            CreateMap<LessonEntity, LessonDto>().ReverseMap();
            CreateMap<double, decimal>().ConvertUsing(d => Convert.ToDecimal(d));
            CreateMap<decimal, double>().ConvertUsing(d => Convert.ToDouble(d));
        }
    }
}
