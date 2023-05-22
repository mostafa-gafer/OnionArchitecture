using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Application.Contracts.Identity;
using Application.Models.Authentication;
using Identity.Authorization;
using Identity.Models;
using Identity.Services;
using System.Text;

namespace Identity
{
    public static class IdentityServiceExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<GoogleAuthSettings>(configuration.GetSection("GoogleAuthSettings"));

            services.AddDbContext<BasketCommerceIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("BasketCommerceIdentityConnectionString"),
                b => b.MigrationsAssembly(typeof(BasketCommerceIdentityDbContext).Assembly.FullName)));

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //       .AddEntityFrameworkStores<RefrigeratorMaintenanceCenterIdentityDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, RefrigeratorMaintenanceCenterIdentityDbContext>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.User.RequireUniqueEmail = true;

                //Here, we allow the user lockout functionality for the new users (it is like that by default but just to state it explicitly). Also, we add two minutes timespan for the lockout and set the number of wrong attempts (three) after which the lockout will take place.
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<BasketCommerceIdentityDbContext>().AddDefaultTokenProviders();

            //we set the lifespan of this token to two hours. for forgetpassword token email.
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                    opt.TokenLifespan = TimeSpan.FromHours(2));
            //---

            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<Application.Contracts.Identity.IAuthenticationService, Services.AuthenticationService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };

                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            InjectPolices.AddPolicies(services);
        }
    }
}
