using App.Domain.Entities;
using App.DTOs;
using App.Features.Commands.CreateContact;
using App.Features.Commands.UpdateContact;
using AutoMapper;

namespace App.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactDto, Contact>();
            CreateMap<Contact, ContactDto>();

            CreateMap<CreateContactCommand, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateContactCommand, Contact>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
