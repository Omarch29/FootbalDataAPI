using System.Collections.Generic;
using AutoMapper;
using Solstice.API.DTOs;
using Solstice.API.models;

namespace Solstice.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Contact, ContactToAddDTO>();
            CreateMap<ContactToAddDTO, Contact>();

            CreateMap<Contact, ContactDTO>()
            .ForMember(dest => dest.Forename, opt => {opt.MapFrom(src => src.Name.Forename());})
            .ForMember(dest => dest.Birthdate, opt => {opt.MapFrom(src => src.Birthdate.ToString("yyyy-MM-dd"));})
            .ForMember(dest => dest.Address, opt => { opt.MapFrom(src => src.Address.ToString());})
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(s => Mapper.Map<ICollection<PhoneNumber>, IList<PhoneNumberDTO>>(s.PhoneNumbers)));
            
            CreateMap<PhoneNumber, PhoneNumberDTO>()
            .ForMember(dest => dest.Type, opt => {opt.MapFrom(src => src.WorkOrPersonal());});

        }


    }
}