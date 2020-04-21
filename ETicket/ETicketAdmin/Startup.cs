using System;
using AutoMapper;
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
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
            services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(x.GetService<ETicketDataContext>()));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ETicketDataContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews().AddFluentValidation();
            services.AddTransient<IValidator<TicketType>, TicketTypeValidator>();
            services.AddTransient<IValidator<Ticket>, TicketValidator>();
            services.AddTransient<IValidator<User>, UserValidator>();
            services.AddTransient<IValidator<Document>, DocumentValidator>();
            services.AddTransient<IValidator<DocumentType>, DocumentTypeValidator>();
            services.AddTransient<IValidator<Privilege>, PrivilegeValidator>();
            services.AddTransient<IValidator<TransactionHistory>, TransactionHistoryValidator>();

            services.AddAutoMapper(typeof(Startup));

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
