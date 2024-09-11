using AutoMapper;
using RoboticsContainer.Core.Models;
using RoboticsContainer.Application.DTOs;

namespace RoboticsContainer.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContainerEntryRequestDTO, ContainerEntry>()
                .ForMember(dest => dest.DateInserted, opt => opt.Ignore());
        }
    }
}
