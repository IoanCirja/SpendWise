using Microsoft.Extensions.DependencyInjection;

using Application.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, DatabaseContext>();

            services.AddScoped<IIdentityHandler, IdentityHandler>();
            services.AddScoped<IPasswordHasher, PasswordHandler>();
            services.AddScoped<IBudgetPlanRepository, BudgetPlanRepository>();
            services.AddScoped<IMonthlyPlanRepository, MonthlyPlanRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IParserData, ParserData>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();


            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            return services;
        }
    }
}
