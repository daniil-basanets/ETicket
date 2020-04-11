using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicket.Models;
using ETicket.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ETicket
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

            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
            services.AddTransient<IUnitOfWork, ETicketData>(x => new ETicketData(x.GetService<ETicketDataContext>()));
            services.AddControllers();
            services.AddSingleton<IMerchant>(merchant);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ETicketDataContext eTicketDataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            eTicketDataContext.EnsureCreated();

            //SeedTransactionHistory(eTicketDataContext);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SeedTransactionHistory(ETicketDataContext eTicketDataContext)
        {
            eTicketDataContext.TransactionHistory.Add(
                new TransactionHistory
                {
                    Count = 1,
                    TotalPrice = 8,
                    ReferenceNumber = "P24A00000000000001",
                    TicketTypeId = 2,
                    Date = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                });

            eTicketDataContext.SaveChanges();
        }
    }
}