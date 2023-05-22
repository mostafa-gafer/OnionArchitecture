

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Contracts.Infrastructure;
using Application.Models.Mail;
using Infrastructure.FileExport;
using Infrastructure.Mail;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<ICsvExporter, CsvExporter>();
            services.AddTransient<IEmailService, EmailService>();

            //SMTP email sender
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
