using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration cofig)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(Options =>
            {
                Options.UseSqlite(cofig.GetConnectionString("DefaultConnection"));

            });
            return services;
        }
    }
}
