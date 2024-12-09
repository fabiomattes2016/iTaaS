using AutoMapper;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;

namespace iTaaS.ConvertLogService.Profiles
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            // Input -> Output
            CreateMap<Source, SourceReadDTO>();
            CreateMap<SourceCreateDTO, Source>();
        }
    }
}
