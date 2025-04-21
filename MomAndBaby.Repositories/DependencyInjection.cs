using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Repositories.Repositories;

namespace MomAndBaby.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            return services;
        }
    }
}
