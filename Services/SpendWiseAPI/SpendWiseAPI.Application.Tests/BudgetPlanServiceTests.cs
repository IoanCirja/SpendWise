using Application.Interfaces;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using FluentAssertions;
using Domain;
using System.Numerics;

namespace SpendWiseAPI.Application.Tests
{
    public class BudgetPlanServiceTests
    {
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IMonthlyPlanRepository _monthlyPlanRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IEmailSender _emailSender;
        private readonly BudgetPlanService _budgetPlanService;
        public BudgetPlanServiceTests()
        {
            this._budgetPlanRepository = Substitute.For<IBudgetPlanRepository>();
            this._monthlyPlanRepository = Substitute.For<IMonthlyPlanRepository>();
            this._authenticationRepository = Substitute.For<IAuthenticationRepository>();
            this._emailSender = Substitute.For<IEmailSender>();
            this._budgetPlanService = new BudgetPlanService(this._budgetPlanRepository, this._monthlyPlanRepository, this._authenticationRepository, this._emailSender);
        }
        [Fact]
        public async Task GetPlans_Should_ReturnListBudgetPlan_When_HaveInfoInDatabase()
        {
            //Arrange
            var budgetPlan = new BudgetPlanGet
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = "Ionut"
            };
            this._budgetPlanRepository.GetPlans().Returns(new List<BudgetPlanGet>()
            {
                budgetPlan
            });

            //Act
            var result = this._budgetPlanService.GetPlans();

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.FirstOrDefault().Should().Be(budgetPlan);
            result.FirstOrDefault().plan_id.Should().Be(Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"));
            result.FirstOrDefault().name.Should().Be("Family Plan");
            result.FirstOrDefault().description.Should().Be("Description");
            result.FirstOrDefault().noCategory.Should().Be(4);
            result.FirstOrDefault().category.Should().Be("Food,Health,Travel,Home");
        }
        [Fact]
        public async Task GetPlans_Should_ReturnEmptyListBudgetPlan_When_HaventInfoInDatabase()
        {
            //Arrange
            this._budgetPlanRepository.GetPlans().Returns(new List<BudgetPlanGet>());

            //Act
            var result = this._budgetPlanService.GetPlans();

            //Assert
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task GetPlan_Should_ReturnListBudgetPlan_When_HaveInfoInDatabaseAndIDIsValid()
        {
            //Arrange
            var budgetPlan = new BudgetPlanGet
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = "Ionut"
            };
            this._budgetPlanRepository.GetPlan(budgetPlan.plan_id).Returns(new List<BudgetPlanGet>()
            {
                budgetPlan
            });

            //Act
            var result = this._budgetPlanService.GetPlan(budgetPlan.plan_id);

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetPlan_Should_ReturnEmptyListBudgetPlan_When_HaveInfoInDatabaseAndIDIsNotValid()
        {
            //Arrange
            var plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C8");
            var budgetPlan = new BudgetPlanGet
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = "Ionut"
            };
            this._budgetPlanRepository.GetPlan(budgetPlan.plan_id).Returns(new List<BudgetPlanGet>());

            //Act
            var result = this._budgetPlanService.GetPlan(budgetPlan.plan_id);

            //Assert
            result.Should().BeEmpty();
            result.Count.Should().Be(0);
        }
        [Fact]
        public async Task GetFivePopularPlan_Should_ReturnListBudgetPlan_When_HaveInfoInDatabase()
        {
            //Arrange
            var budgetPlan1 = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 4
            };
            var budgetPlan2 = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 3
            };
            var budgetPlan3 = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 2
            };
            var budgetPlan4 = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 1
            };
            var budgetPlan5 = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 1
            };
            this._budgetPlanRepository.GetPopularFivePlans().Returns(new List<BudgetPlanGetPopular>()
            {
                budgetPlan1,
                budgetPlan2,
                budgetPlan3,
                budgetPlan4,
                budgetPlan5
            });

            //Act
            var result = this._budgetPlanService.GetPopularFivePlans();

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            result.FirstOrDefault().Should().Be(budgetPlan1);
        }
        [Fact]
        public async Task GetFivePopularPlan_Should_ReturnEmptyListBudgetPlan_When_HaventInfoInDatabased()
        {
            //Arrange
            this._budgetPlanRepository.GetPopularFivePlans().Returns(new List<BudgetPlanGetPopular>());

            //Act
            var result = this._budgetPlanService.GetPopularFivePlans();

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task GetPlansByAdminCreator_Should_ReturnListBudgetPlan_When_HaveInfoInDatabase()
        {
            //Arrange
            var admin_id = Guid.Parse("A13BA2D4-4A1E-430B-96D3-EB3743EDA0C7");
            var budgetPlan1 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var budgetPlan2 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            this._budgetPlanRepository.GetPlansByAdminCreator(admin_id).Returns(new List<BudgetPlan>()
            {
                budgetPlan1,
                budgetPlan2,
            });

            //Act
            var result = this._budgetPlanService.GetPlansByAdminCreator(admin_id);

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FirstOrDefault().Should().Be(budgetPlan1);
        }
        [Fact]
        public async Task GetPlansByAdminCreator_Should_ReturnEmptyListBudgetPlan_When_HaventInfoInDatabased()
        {
            //Arrange
            var admin_id = Guid.Parse("A13BA2D4-4A1E-430B-96D3-EB3743EDA0C7");
            this._budgetPlanRepository.GetPlansByAdminCreator(admin_id).Returns(new List<BudgetPlan>());

            //Act
            var result = this._budgetPlanService.GetPlansByAdminCreator(admin_id);

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task DeletePlanById_Should_ReturnString_When_HaveInfoInDatabase()
        {
            //Arrange
            var plan_id = Guid.Parse("A13BA2D4-4A1E-430B-96D3-EB3743EDA0C7");
            this._budgetPlanRepository.GetPlanById(plan_id).Returns(new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            });
            this._budgetPlanService.DeletePlanById(plan_id).Returns("Plan with Id: A13BA2D4 - 4A1E - 430B - 96D3 - EB3743EDA0C7 was deleted");
            this._monthlyPlanRepository.CancelMonthlyPlansByPlanId(plan_id).Returns(true);

            //Act
            var result = this._budgetPlanService.DeletePlanById(plan_id);

            //Assert
            result.Should().NotBeNull();
        }
        public async Task DeletePlanByName_Should_ReturnListString_When_HaveInfoInDatabase()
        {
            //Arrange
            var plan_name = "Family Plan";
            this._budgetPlanRepository.GetPlanByName(plan_name).Returns(new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            });
            this._budgetPlanService.DeletePlanByName(plan_name).Returns("Plan with Id: A13BA2D4 - 4A1E - 430B - 96D3 - EB3743EDA0C7 was deleted");
            this._monthlyPlanRepository.CancelMonthlyPlansByPlanId(Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")).Returns(true);

            //Act
            var result = this._budgetPlanService.DeletePlanByName(plan_name);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task EditPlanById_Should_ReturnBudgetPlan_When_InfoIsCorrect()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var budgetPlan1 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description1",
                noCategory = 5,
                category = "Food,Health,Travel,Home?Pet",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._budgetPlanRepository.GetPlanByName(budgetPlan.name).Returns(budgetPlan1);
            this._budgetPlanRepository.GetPlanById(plan_id).Returns(budgetPlan1);
            this._budgetPlanRepository.EditPlanById(budgetPlan, plan_id).Returns(budgetPlan);

            //Act
            var result = this._budgetPlanService.EditPlanByPlanId(budgetPlan, plan_id);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task EditPlanById_Should_ReturnException_When_TwoPlanHasSameName()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan1",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var budgetPlan1 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description1",
                noCategory = 5,
                category = "Food,Health,Travel,Home?Pet",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._budgetPlanRepository.GetPlanByName(budgetPlan.name).Returns(budgetPlan1);
            this._budgetPlanRepository.GetPlanById(plan_id).Returns(budgetPlan1);
            this._budgetPlanRepository.EditPlanById(budgetPlan, plan_id).Returns(budgetPlan);

            //Act
            Func<Task<BudgetPlan>> act = async () => await this._budgetPlanService.EditPlanByPlanId(budgetPlan, plan_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Plan with this name already exists", ex.Message);
        }
        [Fact]
        public async Task EditPlanByName_Should_ReturnBudgetPlan_When_InfoIsCorrect()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var budgetPlan1 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description1",
                noCategory = 5,
                category = "Food,Health,Travel,Home?Pet",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var name = "Family Plan";
            this._budgetPlanRepository.GetPlanByName(name).Returns(budgetPlan1,budgetPlan1);
            this._budgetPlanRepository.EditPlanByName(budgetPlan, name).Returns(budgetPlan);

            //Act
            var result = this._budgetPlanService.EditPlanByPlanName(budgetPlan, name);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task EditPlanByName_Should_ReturnException_When_TwoPlanHasSameName()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan1",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var budgetPlan1 = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan1",
                description = "Description1",
                noCategory = 5,
                category = "Food,Health,Travel,Home?Pet",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            var name = "Family Plan";
            this._budgetPlanRepository.GetPlanByName(name).Returns(budgetPlan1);
            this._budgetPlanRepository.GetPlanByName(budgetPlan.name).Returns(budgetPlan1);
            this._budgetPlanRepository.EditPlanByName(budgetPlan, name).Returns(budgetPlan);

            //Act
            Func<Task<BudgetPlan>> act = async () => await this._budgetPlanService.EditPlanByPlanName(budgetPlan, name);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Plan with this name already exists", ex.Message);
        }
        [Fact]
        public async Task AddPlan_Should_ReturnTrue_When_InfoIsCorrect()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            this._budgetPlanRepository.GetPlanByName(budgetPlan.name).Returns((BudgetPlan)null);
            this._budgetPlanRepository.AddPlan(budgetPlan).Returns(true);
            List<string> emails = new List<string>() { "ionut.vasile@gmail.com" };
            string imageDataUrl = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string subject = $"Introducing Our New Budget Plan: {budgetPlan.name}";
            string body = $@"
                               <html>
                                <body>
                                <img src='{imageDataUrl}' alt='Logo' />
                                <p><strong>Dear Subscriber,</strong></p>
                                <p>We are excited to announce that a new budget plan, named <strong>{budgetPlan.name}</strong>, has been added to our collection of financial tools on SpendWise!</p>
                                <p>Description:</p>
                                <p>This plan, <strong>{budgetPlan.name}</strong>, is designed to help you streamline your expenses and optimize your savings. It offers a flexible approach to managing your finances, suitable for various income levels and spending habits.</p>
                                <p>Categories Included:</p>
                                <p>The <strong>{budgetPlan.name}</strong> budget plan is structured around {budgetPlan.noCategory} key categories:</p>
                                <ul>";

            var categories = budgetPlan.category.Split(',');

            foreach (var category in categories)
            {
                body += $"<li>{category.Trim()}</li>";
            }


            body += @"
                            </ul>
                            <p>We believe this new budget plan will be a valuable addition to your financial toolkit, making it easier than ever to achieve your financial goals.</p>
                            <p>Have a great day!</p>
                            <p>Warm regards</p>
                            <p>The SpendWise Team</p>
                            <p>Smart Spending, Simplified</p>
                        </body>
                    </html>";
            this._emailSender.SendEmailAsync(emails, subject, body);

            //Act
            var result = this._budgetPlanService.AddNewPlan(budgetPlan);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task AddPlan_Should_ReturnFalse_When_AddPlanReturnFalse()
        {
            //Arrange
            var budgetPlan = new BudgetPlan
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                isActive = true,
                creationDate = DateTime.Parse("2024-07-29 18:32:00"),
                created_by = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7")
            };
            this._budgetPlanRepository.GetPlanByName(budgetPlan.name).Returns((BudgetPlan)null);
            this._budgetPlanRepository.AddPlan(budgetPlan).Returns(false);
            List<string> emails = new List<string>() { "ionut.vasile@gmail.com" };
            string imageDataUrl = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string subject = $"Introducing Our New Budget Plan: {budgetPlan.name}";
            string body = $@"
                               <html>
                                <body>
                                <img src='{imageDataUrl}' alt='Logo' />
                                <p><strong>Dear Subscriber,</strong></p>
                                <p>We are excited to announce that a new budget plan, named <strong>{budgetPlan.name}</strong>, has been added to our collection of financial tools on SpendWise!</p>
                                <p>Description:</p>
                                <p>This plan, <strong>{budgetPlan.name}</strong>, is designed to help you streamline your expenses and optimize your savings. It offers a flexible approach to managing your finances, suitable for various income levels and spending habits.</p>
                                <p>Categories Included:</p>
                                <p>The <strong>{budgetPlan.name}</strong> budget plan is structured around {budgetPlan.noCategory} key categories:</p>
                                <ul>";

            var categories = budgetPlan.category.Split(',');

            foreach (var category in categories)
            {
                body += $"<li>{category.Trim()}</li>";
            }


            body += @"
                            </ul>
                            <p>We believe this new budget plan will be a valuable addition to your financial toolkit, making it easier than ever to achieve your financial goals.</p>
                            <p>Have a great day!</p>
                            <p>Warm regards</p>
                            <p>The SpendWise Team</p>
                            <p>Smart Spending, Simplified</p>
                        </body>
                    </html>";
            this._emailSender.SendEmailAsync(emails, subject, body);

            //Act
            var result = this._budgetPlanService.AddNewPlan(budgetPlan);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
