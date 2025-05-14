using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.ExpertModel;
using MomAndBaby.Services.DTO.JournalModel;
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
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Expert, ExpertViewModel>().ReverseMap();
            CreateMap<Pagination<User>, Pagination<UserViewModel>>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<Journal, JournalViewModel>().ReverseMap();
            CreateMap<Journal, JournalDto>().ReverseMap();
            CreateMap<Pagination<Journal>, Pagination<JournalViewModel>>().ReverseMap();
            CreateMap<ExUserViewModel, User>().ReverseMap();
            CreateMap<ExpertProfileViewModel, Expert>().ReverseMap();
            CreateMap<Pagination<Expert>, Pagination<ExpertProfileViewModel>>().ReverseMap();
            CreateMap<Expert, UpdateExpertModel>().ReverseMap();
        }
    }
}
