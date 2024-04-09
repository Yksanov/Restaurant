using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restoraunt.Areas.Identity.Data;
using Restoraunt.Data;

[assembly: HostingStartup(typeof(Restoraunt.Areas.Identity.IdentityHostingStartup))]
namespace Restoraunt.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<RestDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("RestDbContextConnection")));

                services.AddDefaultIdentity<RestorauntUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<RestDbContext>();
            });
        }
    }
}