using AutoMapper;
using Pinewood.Domain.Entities;
using Pinewood.Shared.Dtos;

namespace Pinewood.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //// Configure mapping from CreateCustomerDto to Customer
            //CreateMap<CreateCustomerDto, Customer>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is generated in service
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore()) // Set in service
            //    .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore()); // Set in service

            CreateMap<Customer, CustomerDto>();
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}
