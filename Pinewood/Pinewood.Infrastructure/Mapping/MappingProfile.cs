using AutoMapper;
using Pinewood.Domain.Entities;
using Pinewood.Shared.Dtos;

namespace Pinewood.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}
