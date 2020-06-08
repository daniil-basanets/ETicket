using System;
using System.Collections.Generic;
using ETicket.ApplicationServices.Logger;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services.Transaction;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.WebAPI.Models;
using ETicket.WebAPI.Models.Identity;
using ETicket.WebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            var publicKey = Configuration["Merchant:Public_Key"];
            var privateKey = Configuration["Merchant:Private_Key"];

            IMerchant merchant = new Merchant
            {
                PublicKey = publicKey,
                PrivateKey = privateKey
            };

            services.AddDbContext<ETicketDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
            services.AddTransient<IUnitOfWork, UnitOfWork>(e => new UnitOfWork(e.GetService<ETicketDataContext>()));

            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ITicketTypeService, TicketTypeService>();
            services.AddTransient<ICarrierService, CarrierService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IDocumentTypesService, DocumentTypesService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IPriceListService, PriceListService>();
            services.AddTransient<IAreaService, AreaService>();
            services.AddTransient<ITicketVerificationService, TicketVerificationService>();
            services.AddTransient<IPrivilegeService, PrivilegeService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IRouteService, RouteService>();
            services.AddTransient<IStationService, StationService>();

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ETicketDataContext>()
               .AddDefaultTokenProviders()
               .AddTokenProvider(AuthOptions.ISSUER, typeof(DataProtectorTokenProvider<IdentityUser>));

            services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
            });
            const string jwtSchemeName = "JwtBearer";
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = jwtSchemeName;
                    options.DefaultChallengeScheme = jwtSchemeName;
                })
                .AddJwtBearer(jwtSchemeName, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            services.AddControllers();
            services.AddSingleton<IMerchant>(merchant);

            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISecretCodeService, SecretCodeService>();


            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETicket API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.<br>" +
                                  "Use POST '/api/authentication/token' to GET Bearer token.<br>" +
                                  "Enter 'Bearer' [space] and then your token in the text input below.<br>" +
                                  "Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

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