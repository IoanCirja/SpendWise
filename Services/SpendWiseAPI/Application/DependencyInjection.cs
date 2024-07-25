using Microsoft.Extensions.DependencyInjection;

using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddScoped<BudgetPlanService>();
            services.AddScoped<MonthlyPlanService>();
            services.AddScoped<NewsLetterService>();
            services.AddScoped<ContactUsService>();
            services.AddScoped<TransactionsService>();

            return services;
        }
    }
}
