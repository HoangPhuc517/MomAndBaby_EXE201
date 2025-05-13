using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;
using MomAndBaby.Services.Mapping;
using MomAndBaby.Services.Services;

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

            //Config AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            //Config MailSetting
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;
        }
    }
}
