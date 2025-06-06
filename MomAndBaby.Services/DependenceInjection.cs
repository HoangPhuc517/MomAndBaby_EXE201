﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MomAndBaby.Services.BackgroundServices;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;
using MomAndBaby.Services.Mapping;
using MomAndBaby.Services.Services;
using Net.payOS;

namespace MomAndBaby.Services
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJournalService, JournalService>();
            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IUserPackageService, UserPackageService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPayOsService, PayOsService>();


            //Config Coudinary
            var cloudinarySettings = configuration.GetRequiredSection("Cloudinary");
            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]
            );

            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
            services.AddScoped<UploadFile>();

            //Config PayOsSettings
            services.Configure<PayOsSettings>(configuration.GetSection("PayOs"));

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<PayOsSettings>>().Value;
                if (string.IsNullOrEmpty(settings.ClientID) ||
                    string.IsNullOrEmpty(settings.APIKey) ||
                    string.IsNullOrEmpty(settings.ChecksumKey))
                {
                    throw new InvalidOperationException("PayOS configuration is missing or incomplete");
                }
                return new PayOS(settings.ClientID, settings.APIKey, settings.ChecksumKey);
            });

            //Config AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            //Config MailSetting
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            //Config background services
            services.AddHostedService<ExpiredDealProcessor>();

            return services;
        }
    }
}
