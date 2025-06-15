using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.AppointmentModel;
using MomAndBaby.Services.DTO.BlogModel;
using MomAndBaby.Services.DTO.ChatModel;
using MomAndBaby.Services.DTO.DealModel;
using MomAndBaby.Services.DTO.ExpertModel;
using MomAndBaby.Services.DTO.FeedbackModel;
using MomAndBaby.Services.DTO.JournalModel;
using MomAndBaby.Services.DTO.ServicePackageModel;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.DTO.UserPackageModel;

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
            CreateMap<ServicePackage, PackageModel>().ReverseMap();
            CreateMap<Deal, PackageDealModel>().ReverseMap();
            CreateMap<ServicePackage, PackageViewModel>().ReverseMap();
            CreateMap<Deal, PackageDealViewModel>().ReverseMap();
            CreateMap<Pagination<ServicePackage>, Pagination<PackageViewModel>>().ReverseMap();
            CreateMap<ServicePackage, UpdatePackageModel>().ReverseMap();
            CreateMap<Deal, CreateDealModel>()
                .ForMember(dest => dest.ServicePackageId, opt => opt.MapFrom(src => src.ServicePackageId.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.ServicePackageId, opt => opt.MapFrom(src => Guid.Parse(src.ServicePackageId)));
            CreateMap<Deal, UpdateDealModel>().ReverseMap();
            CreateMap<Deal, DealViewModel>().ReverseMap();
            CreateMap<ServicePackage, SubPackageViewModel>().ReverseMap();
            CreateMap<UserPackage, UserPackageViewModel>().ReverseMap();
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
            CreateMap<Transaction, CreateTransactionDTO>().ReverseMap();
            CreateMap<Pagination<Transaction>, Pagination<TransactionViewModel>>().ReverseMap();
            CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => src.TimeSlot.Time))
                .ForMember(dest => dest.ReportCount, opt => opt.MapFrom(src => src.Reports.Count()))
                .ReverseMap();
            CreateMap<Feedback, FeedbackAppointmentVM>().ReverseMap();
            CreateMap<Pagination<Appointment>, Pagination<AppointmentViewModel>>()
                .ReverseMap();
            CreateMap<Appointment, CreateAppointmentDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<AppointmentTypeEnum>(src.Type)))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            CreateMap<RegisterAdminDTO, User>().ReverseMap();
            CreateMap<Feedback, FeedbackViewModel>().ReverseMap();
            CreateMap<Feedback, CreateFeedbackDTO>().ReverseMap();
            CreateMap<Pagination<Feedback>, Pagination<FeedbackViewModel>>().ReverseMap();
            CreateMap<Blog, CreateBlogModel>().ReverseMap();
            CreateMap<Blog, ResponseBlogModel>().ReverseMap();
            CreateMap<Pagination<Blog>, Pagination<ResponseBlogModel>>().ReverseMap();
            CreateMap<Comment, CommentModel>().ReverseMap();
            CreateMap<Like, LikeModel>().ReverseMap();
            CreateMap<Report, ReportModel>().ReverseMap();
            CreateMap<ChatHub, ResponseChatHup>().ReverseMap();
            CreateMap<ChatMessage, ResponseChatMessage>().ReverseMap();
            CreateMap<Appointment, CalendarExpertViewModel>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.TimeSlot.Time))
                .ReverseMap();
        }
    }
}
