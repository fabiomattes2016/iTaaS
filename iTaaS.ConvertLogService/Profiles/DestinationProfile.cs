using AutoMapper;
using iTaaS.ConvertLogService.DTOs;
using iTaaS.ConvertLogService.Models;

namespace iTaaS.ConvertLogService.Profiles
{
    public class DestinationProfile : Profile
    {
        public DestinationProfile()
        {
            CreateMap<Destination, DestinationReadDTO>();
            CreateMap<DestinationCreateDTO, Destination>();
            CreateMap<DestinationReadDTO, Destination>();
        }
    }
}
