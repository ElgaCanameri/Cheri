using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using FashionApp.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FashionApp.DAL
{
    public static class Startup
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("AppDbContextConnection"));
            });
            //services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddRoles<AppRole>()
            //        .AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void StartApp(string connectionString)
        {
            Utils.ConnectionString = connectionString;
        }
    }
}
