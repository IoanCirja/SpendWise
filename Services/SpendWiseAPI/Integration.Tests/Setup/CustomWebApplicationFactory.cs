using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using Application.Interfaces;
using NSubstitute;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Application.Services;
using Infrastructure.Repositories;

namespace Integration.Tests.Setup
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public IPasswordHasher MockPasswordHasher { get; private set; }
        public IAuthenticationRepository MockAuthenticationRepository { get; private set; }
        public IIdentityHandler MockIdentityHandler { get; private set; }
        public AuthorizationService MockAuthenticationService { get; private set; }

        public IBudgetPlanRepository MockBudgetPlanRepository { get; private set; }

        public IContactUsRepository MockContactUsRepository { get; private set; }
        public IMonthlyPlanRepository MockMonthlyPlanRepository { get; private set; } 

        public ITransactionsRepository MockTransactionsRepository { get; private set; }

        public MonthlyPlanService MockMonthlyPlanService { get; private set; }

        public INewsletterRepository MockNewsLetterRepostiroy {  get; private set; }
        public IParserData MockParserData{ get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Crearea mock-urilor folosind NSubstitute
                MockPasswordHasher = Substitute.For<IPasswordHasher>();
                MockAuthenticationRepository = Substitute.For<IAuthenticationRepository>();
                MockIdentityHandler = Substitute.For<IIdentityHandler>();

                MockAuthenticationService = new AuthorizationService(MockPasswordHasher, MockAuthenticationRepository, MockIdentityHandler);
                MockBudgetPlanRepository = Substitute.For<IBudgetPlanRepository>();
                MockContactUsRepository = Substitute.For<IContactUsRepository>();
                MockMonthlyPlanRepository = Substitute.For<IMonthlyPlanRepository>();
                MockTransactionsRepository = Substitute.For<ITransactionsRepository>();
                MockNewsLetterRepostiroy = Substitute.For<INewsletterRepository>();

                MockMonthlyPlanService = new MonthlyPlanService(MockMonthlyPlanRepository, MockBudgetPlanRepository,MockTransactionsRepository);
                MockParserData = Substitute.For<IParserData>();

                // Înlocuirea serviciilor reale cu mock-urile
                services.AddSingleton(MockPasswordHasher);
                services.AddSingleton(MockAuthenticationRepository);
                services.AddSingleton(MockIdentityHandler);
                services.AddSingleton(MockAuthenticationService);
                services.AddSingleton(MockBudgetPlanRepository);
                services.AddSingleton(MockContactUsRepository);
                services.AddSingleton(MockMonthlyPlanRepository);
                services.AddSingleton(MockMonthlyPlanService);
                services.AddSingleton(MockTransactionsRepository);
                services.AddSingleton(MockNewsLetterRepostiroy);
                services.AddSingleton(MockParserData);
            });
        }


    }
}
