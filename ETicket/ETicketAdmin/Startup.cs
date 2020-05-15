using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Logger;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services.Transaction;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.WebAPI.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETicket.Admin
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
            services.AddControllersWithViews();
            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
            services.AddTransient<IUnitOfWork, UnitOfWork>(u => new UnitOfWork(u.GetService<ETicketDataContext>()));

            services.AddTransient<ITransactionAppService, TransactionAppService>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ETicketDataContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews().AddFluentValidation();
            services.AddTransient<IValidator<TicketTypeDto>, TicketTypeValidator>();
            services.AddTransient<IValidator<TicketDto>, TicketValidator>();
            services.AddTransient<IValidator<UserDto>, UserValidator>();
            services.AddTransient<IValidator<DocumentDto>, DocumentValidator>();
            services.AddTransient<IValidator<DocumentTypeDto>, DocumentTypeValidator>();
            services.AddTransient<IValidator<PrivilegeDto>, PrivilegeValidator>();
            services.AddTransient<IValidator<TransactionHistoryDto>, TransactionHistoryValidator>();


            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDocumentTypesService, DocumentTypesService>();
            services.AddTransient<ITicketTypeService, TicketTypeService>();
            services.AddTransient<ICarrierService, CarrierService>();
            services.AddTransient<IPrivilegeService, PrivilegeService>();
            services.AddTransient<IAreaService, AreaService>();


            services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
            });

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Account/Login";
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    options.SlidingExpiration = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ETicketDataContext eTicketDataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            eTicketDataContext.EnsureCreated();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
