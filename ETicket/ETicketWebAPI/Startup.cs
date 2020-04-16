using ETicketDataAccess.Domain;
using ETicketDataAccess.Domain.Interfaces;
using ETicketWebAPI.Models;
using ETicketWebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETicketWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var merchantId = Configuration["Merchant:MerchantId"];
            var password = Configuration["Merchant:Password"];

            IMerchant merchant = new Merchant
            {
                MerchantId = int.Parse(merchantId),
                Password = password
            };

            var cardNumber = Configuration["MerchantSettings:CardNumber"];

            IMerchantSettings merchantSettings = new MerchantSettings
            {
                CardNumber = cardNumber
            };

            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
            services.AddTransient<IUnitOfWork, UnitOfWork>(e => new UnitOfWork(e.GetService<ETicketDataContext>()));
            
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ETicketDataContext>();
            services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
            });
            services.AddControllers();
            services.AddSingleton<IMerchant>(merchant);
            services.AddSingleton<IMerchantSettings>(merchantSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ETicketDataContext eTicketDataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            eTicketDataContext.EnsureCreated();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}