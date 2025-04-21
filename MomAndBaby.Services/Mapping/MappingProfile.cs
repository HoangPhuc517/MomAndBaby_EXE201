using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Services.DTO.UserModel;

namespace MomAndBaby.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterCustomerDTO>().ReverseMap();
            CreateMap<User, RegisterExpertDTO>().ReverseMap();
            CreateMap<ExpertDTO, Expert>().ReverseMap();
        }
    }
}
