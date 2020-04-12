using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Interfaces;
using ETicket.Models;
using ETicket.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETicket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var merchantId = Configuration["Merchant:MerchantId"];
            var password = Configuration["Merchant:Password"];

            IMerchant merchant = new Merchant
            {
                MerchantId = int.Parse(merchantId),
                Password = password
            };

            var databaseConnectionString = Configuration.GetConnectionString("DatabaseConnectionString");

            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(databaseConnectionString));
            services.AddTransient<IUnitOfWork, ETicketData>(x => new ETicketData(x.GetService<ETicketDataContext>()));
            services.AddSingleton<IMerchant>(merchant);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ETicketDataContext eTicketDataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            eTicketDataContext.EnsureCreated();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}