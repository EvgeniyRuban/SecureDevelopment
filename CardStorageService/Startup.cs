using System.Text;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using FluentValidation;
using CardStorageService.DAL;
using CardStorageService.Domain;
using CardStorageService.Services;

namespace CardStorageService
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
            var mapperConfigurations = new MapperConfiguration(mp => mp.AddProfile<MappingProfile>()); 
            var mapper = mapperConfigurations.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IValidator<AccountToCreate>, AccountToCreateValidator>();
            services.AddScoped<IValidator<CardToCreate>, CardToCreateValidator>();
            services.AddScoped<IValidator<ClientToCreate>, ClientToCreateValidator>();
            services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();

            services.AddDbContext<CardsStorageServiceDbContext>(options =>
            {
                options.UseSqlServer(CacheProvider.GetConnectionFromCache().ToString());
            });

            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<ICardsRepository, CardsRepository>();
            services.AddScoped<ICardsService, CardsService>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IAccountsSessionsRepository, AccountsSessionsRepository>();
            services.AddScoped<IAccountsSessionsService, AccountsSessionsService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            services.AddControllers();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationService.SercretCode)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CardStorageService", Version = "v1" });
                c.CustomOperationIds(SwaggerUtils.OperationIdProvider);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 1234abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardStorageService v1"));
            }

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