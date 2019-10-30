using System;
using System.Text;
using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services;
using MAuthen.Api.Services.Implimentation;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Data;
using MAuthen.Data.Repositories.Implementation;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MAuthen.Api
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
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IPasswordProcessor, PasswordProcessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContextPool<MAuthenContext>(options =>
              options.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=MAuthen;Integrated Security=True"));

            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);

            services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        LifetimeValidator =
                            (before, expires, verifiedToken, parameters) =>
                            {
                                if (before.HasValue && before.Value > DateTime.UtcNow) { return true; }
                                if (expires.HasValue && expires.Value >= DateTime.UtcNow) { return true; }
                                return false;
                            },
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        NameClaimType = "UserName",
                        RoleClaimType = "Role",
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                    };
                });

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISecretRepository, SecretRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.Configure<JwtOptions>(jwtSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(options=> options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
            );
            app.UseAuthentication();
            app.UseMiddleware<TokenManagerMiddleware>();
            app.UseMvc();
        }
    }
}
