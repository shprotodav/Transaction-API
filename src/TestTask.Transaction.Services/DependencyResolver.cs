using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using TestTask.Transaction.BL.Services;
using TestTask.Transaction.BL.Validators;
using TestTask.Transaction.Common;
using TestTask.Transaction.Common.IRepositories;
using TestTask.Transaction.DAL;
using TestTask.Transaction.DAL.Repositories;
using TestTask.Transaction.DAL.Repositories.Main;

namespace TestTask.Transaction.Services
{
    public static class DependencyResolver
    {
        private static void DefaultNetInjectors(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
        }

        public static void Resolve(IServiceCollection services, IConfiguration configuration)
        {
            var appSettingSection = configuration.GetSection("AppSettings");
            DefaultNetInjectors(services, appSettingSection);

            HttpClientServices(services, appSettingSection);
            Repositories(services);
            Services(services);
            Validators(services);
        }

        private static void HttpClientServices(IServiceCollection services, IConfigurationSection appSettingSection)
        {
            services.AddHttpClient("Identity", client =>
            {
                client.BaseAddress = new Uri(appSettingSection.GetValue<string>("IdentityUrl"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddHttpClient<IAuthService, AuthService>("Identity");
        }

        private static void Validators(IServiceCollection services)
        {
            services.AddScoped<ITransactionValidator, TransactionValidator>();
        }

        private static void Services(IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
        }

        private static void Repositories(IServiceCollection services)
        {
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IHelperRepository, HelperRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }

}
