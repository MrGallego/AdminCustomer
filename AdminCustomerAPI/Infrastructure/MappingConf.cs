using AdminCustomerAPI.Repository.Dto;
using AdminCustomerAPI.Repository.Models;
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
