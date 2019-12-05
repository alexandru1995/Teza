using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using TestIntegrationApplication.Helpers;
using TestIntegrationApplication.Models;
using TestIntegrationApplication.Services;
using TestIntegrationApplication.Services.Implementation;
using TestIntegrationApplication.Services.Interfaces;

namespace TestIntegrationApplication
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
            services.AddHttpClient();
            services.AddSingleton<IJwtToken, JwtToken>();

            var secret = Configuration.GetSection("AuthenticationRequest:secret");
            services.Configure<AuthenticationRequestModel>(Configuration.GetSection("AuthenticationRequest"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, TokenManager>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    LifetimeValidator =
                        (before, expires, verifiedToken, parameters) =>
                        {
                            if (before.HasValue && before.Value > DateTime.UtcNow) { return true; }
                            if (expires.HasValue && expires.Value >= DateTime.UtcNow) { return true; }
                            return false;
                        },
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.Value)),
                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.Value)),

                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenInvalidLifetimeException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            })
                .AddCookie();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<TokenManagerMiddleware>();
            app.UseAuthorization();
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
