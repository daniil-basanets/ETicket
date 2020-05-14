using ETicket.ApplicationServices.Logger;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.WebAPI.Models;
using ETicket.WebAPI.Models.Interfaces;
using ETicket.WebAPI.Services.TicketsService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ETicket.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LoggerService.Initialize();
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

            services.AddTransient<ICarrierService, CarrierService>();

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

            services.AddTransient<ITicketVerificationService, TicketVerificationService>();
            services.AddTransient<Services.TicketsService.ITicketVerifyService, Services.TicketsService.TicketVerifyService>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETicket API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ETicketDataContext eTicketDataContext)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ETicket");
                c.RoutePrefix = string.Empty;
            });

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