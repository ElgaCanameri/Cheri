using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.Services;
using FashionApp.DAL.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FashionApp.BLL
{
    internal static class Utils
    {
        public static string LogsDirectory { get; set; }
    }
    public static class Startup
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            DAL.Startup.RegisterServices(services, configuration);
			services.AddSingleton<ILoggerService, LoggerService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IFavouritesService, FavouritesService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartProductService, CartProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
			services.AddHostedService<LogsService>();
		}
		public static void Start(string logsDir,
            string connectionString)
        {
            Utils.LogsDirectory = logsDir;
            DAL.Startup.StartApp(connectionString);
        }
    }
}
