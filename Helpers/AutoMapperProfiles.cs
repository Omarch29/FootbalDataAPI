using System;
using System.Collections.Generic;
using AutoMapper;
using FootbalDataAPI.DTOs;
using FootbalDataAPI.models;

namespace Football.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {         
            CreateMap<CompetitionDTO, Competition>()
            .ForMember(dest => dest.AreaName, opt => {opt.MapFrom(src => src.Area.Name);});

            CreateMap<TeamDTO, Team>()
            .ForMember(dest => dest.AreaName, opt => {opt.MapFrom(src => src.Area.Name);});
            
            CreateMap<PlayerDTO, Player>()
            .ForMember(dest => dest.DateOfBirth, opt => {opt.MapFrom(src => Convert.ToDateTime(src.DateOfBirth));});

        }
    }
}