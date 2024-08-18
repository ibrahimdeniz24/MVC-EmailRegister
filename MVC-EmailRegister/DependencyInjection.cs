
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_EmailRegister.MailServices;
using MVC_EmailRegister.Models;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace MVC_EmailRegister
{
    public  static class DependencyInjection
    {
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer("Data Source=DENIZ\\SQL;Initial Catalog=HS-15-EMailRegister;Integrated Security=True;TrustServerCertificate=True");
            });

            

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IMailService, MailService>();
            return services;

        }


    }
}

