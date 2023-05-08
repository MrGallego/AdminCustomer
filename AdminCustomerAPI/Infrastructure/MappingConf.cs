using AdminCustomerAPI.Models.Dto;
using AdminCustomerAPI.Models.Models;
using AutoMapper;

namespace AdminCustomerAPI.Infrastructure
{
    public class MappingConf:Profile
    {
        public MappingConf()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();

        }

    }
}
