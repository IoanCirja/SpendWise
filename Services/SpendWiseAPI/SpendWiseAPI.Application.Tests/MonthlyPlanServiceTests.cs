using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace SpendWiseAPI.Application.Tests
{
    public class MonthlyPlanServiceTests
    {
        private readonly IMonthlyPlanRepository _monthlyPlanRepository;
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly MonthlyPlanService _monthlyPlanService;
        public MonthlyPlanServiceTests()
        {
            this._monthlyPlanRepository=Substitute.For<IMonthlyPlanRepository>();
            this._budgetPlanRepository=Substitute.For<IBudgetPlanRepository>();
            this._transactionsRepository=Substitute.For<ITransactionsRepository>();
            this._monthlyPlanService=new MonthlyPlanService(this._monthlyPlanRepository,this._budgetPlanRepository,this._transactionsRepository);
        }
        [Fact]
        public async Task AddMonthlyPlan_Should_ReturnTrue_When_InfoIsCorrect()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.VerifyUserHasPlanActive(monthlyPlan.user_id).Returns(false);
            this._monthlyPlanRepository.AddMonthlyPlans(monthlyPlan).Returns(true);

            //Act
            var result = await this._monthlyPlanService.AddMonthlyPlans(monthlyPlan);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task AddMonthlyPlan_Should_ReturnException_When_UserAlreadyHaveAPlanActive()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.VerifyUserHasPlanActive(monthlyPlan.user_id).Returns(true);
            this._monthlyPlanRepository.AddMonthlyPlans(monthlyPlan).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._monthlyPlanService.AddMonthlyPlans(monthlyPlan);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User already have a current plan active", ex.Message);
        }
        [Fact]
        public async Task AddMonthlyPlan_Should_ReturnFalse_When_MonthlyPlanAddFailed()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.VerifyUserHasPlanActive(monthlyPlan.user_id).Returns(false);
            this._monthlyPlanRepository.AddMonthlyPlans(monthlyPlan).Returns(false);

            //Act
            var result = await this._monthlyPlanService.AddMonthlyPlans(monthlyPlan);

            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task CancelMonthlyPlan_Should_ReturnTrue_When_InfoIsCorrect()
        {
            //Arrange
            var monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._monthlyPlanRepository.CancelMonthlyPlan(monthlyPlan_id).Returns(true);

            //Act
            var result = await this._monthlyPlanService.CancelMonthlyPlan(monthlyPlan_id);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task CancelMonthlyPlan_Should_ReturnFalse_When_CancelPlanFailed()
        {
            //Arrange
            var monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._monthlyPlanRepository.CancelMonthlyPlan(monthlyPlan_id).Returns(false);

            //Act
            var result = await this._monthlyPlanService.CancelMonthlyPlan(monthlyPlan_id);

            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task GetHistoryMonthlyPlans_Should_ReturnListMonthlyPlan_When_InfoIsCorrect()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetHistoryPlans(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetNameDate>()
            {
                new MonthlyPlanGetNameDate
                {
                    monthlyPlan_id=monthlyPlan.monthlyPlan_id,
                    plan_name="Family Plan",
                    date=monthlyPlan.date
                }
            });

            //Act
            var result = this._monthlyPlanService.GetHistoryPlans(monthlyPlan.user_id);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetHistoryMonthlyPlans_Should_ReturnListEmpty_When_UserDoesntHaveMonthlyPlanInHistory()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetHistoryPlans(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetNameDate>());

            //Act
            var result = this._monthlyPlanService.GetHistoryPlans(monthlyPlan.user_id);

            //Assert
            result.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
        [Fact]
        public async Task GetMonthlyPlanFromHistory_Should_ReturnListMonthlyPlan_When_InfoIsCorrect()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(monthlyPlan.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id=monthlyPlan.monthlyPlan_id,
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name="Family Plan",
                    date=monthlyPlan.date,
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });

            //Act
            var result = this._monthlyPlanService.GetMonthlyPlanFromHistory(monthlyPlan.monthlyPlan_id);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetMonthlyPlanFromHistory_Should_ReturnEmptyListMonthlyPlan_When_UserDoesntHavePlanInHistory()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(monthlyPlan.monthlyPlan_id).Returns(new List<MonthlyPlanGet>());

            //Act
            var result = this._monthlyPlanService.GetMonthlyPlanFromHistory(monthlyPlan.monthlyPlan_id);

            //Assert
            result.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
        [Fact]
        public async Task GetCurrentMonthlyPlanFrom_Should_ReturnListMonthlyPlan_When_InfoIsCorrect()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlanGet
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_name = "Family Plan",
                description = "description",
                noCategory = 6,
                category = "Food,Travel,Health,Pet,School,Rent",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetDateID>()
            {
                new MonthlyPlanGetDateID{
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    date = DateTime.Parse("2024-07-29 18:32:00")
                }
            });
            this._monthlyPlanRepository.FinishedMonthlyPlan(monthlyPlan.monthlyPlan_id).Returns(true);
            this._monthlyPlanRepository.GetCurrentPlan(monthlyPlan.user_id).Returns(new List<MonthlyPlanGet>
            {
                monthlyPlan
            });

            //Act
            var result = this._monthlyPlanService.GetCurrentPlan(monthlyPlan.user_id);

            //Assert
            result.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetCurrentMonthlyPlanFrom_Should_ReturnEmptyListMonthlyPlan_When_UserDoesntHaveAPlan()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetDateID>());
            this._monthlyPlanRepository.FinishedMonthlyPlan(monthlyPlan.monthlyPlan_id).Returns(true);

            //Act
            var result = this._monthlyPlanService.GetCurrentPlan(monthlyPlan.user_id);

            //Assert
            result.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
        [Fact]
        public async Task GetCurrentMonthlyPlanFrom_Should_ReturnEmptyListMonthlyPlan_When_FinishedPlanReturnTrueAndDateIsAbove30Days()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-05-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetDateID>()
            {
                new MonthlyPlanGetDateID{
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    date = DateTime.Parse("2024-05-29 18:32:00")
                }
            });
            this._monthlyPlanRepository.FinishedMonthlyPlan(monthlyPlan.monthlyPlan_id).Returns(true);

            //Act
            var result = this._monthlyPlanService.GetCurrentPlan(monthlyPlan.user_id);

            //Assert
            result.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
        [Fact]
        public async Task GetCurrentMonthlyPlanFrom_Should_ReturnException_When_FinishedPlanReturnFalseAndDateIsAbove30Days()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-05-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetDateID>()
            {
                new MonthlyPlanGetDateID{
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    date = DateTime.Parse("2024-05-29 18:32:00")
                }
            });
            this._monthlyPlanRepository.FinishedMonthlyPlan(monthlyPlan.monthlyPlan_id).Returns(false);


            //Act
            Func<List<MonthlyPlanGet>> act = () =>  this._monthlyPlanService.GetCurrentPlan(monthlyPlan.user_id);

            //Assert
            var ex = Assert.Throws<Exception>(act);
            Assert.Equal("errors, problem when finished a monthly plan", ex.Message);
        }
        [Fact]
        public async Task GetCurrentMonthlyPlanFrom_Should_ReturnException_When_UserHaveManyPlansActive()
        {
            //Arrange
            var monthlyPlan = new MonthlyPlan
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                date = DateTime.Parse("2024-07-29 18:32:00"),
                totalAmount = 3500,
                amountSpent = 1750,
                status = "In Progress",
                priceByCategory = "500,750,750,550,450,500",
                spentOfCategory = "250,300,450,300,200,250"
            };
            this._monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(monthlyPlan.user_id).Returns(new List<MonthlyPlanGetDateID>()
            {
                new MonthlyPlanGetDateID{
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    date = DateTime.Parse("2024-07-22 18:32:00")
                },
                new MonthlyPlanGetDateID{
                    monthlyPlan_id = Guid.Parse("B12DA9D0-5B0E-439B-96D9-EB3743EDA0C7"),
                    date = DateTime.Parse("2024-07-29 18:32:00")
                },

            });
            this._monthlyPlanRepository.FinishedMonthlyPlan(monthlyPlan.monthlyPlan_id).Returns(true);


            //Act
            Func<List<MonthlyPlanGet>> act = () => this._monthlyPlanService.GetCurrentPlan(monthlyPlan.user_id);

            //Assert
            var ex = Assert.Throws<Exception>(act);
            Assert.Equal("errors, many plans in progress", ex.Message);
        }
        [Fact]
        public async Task GetDemoMonthlyPlanFrom_Should_ReturnListDemoMonthlyPlan_When_InfoIsCorrect()
        {
            //Arrange
            var plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._monthlyPlanRepository.GetDemoMonthlyPlan(plan_id).Returns(new List<MonthlyPlanDemo>()
            {
                new MonthlyPlanDemo
                {
                    totalAmount = 2500,
                    priceByCategory = "500,500,500,250,250,350,150"
                }
            });

            //Act
            var result = this._monthlyPlanService.GetDemoMonthlyPlan(plan_id);

            //Assert
            result.Should().NotBeNull();
            result.totalAmount.Should().Be(2500);
        }
        [Fact]
        public async Task GetDemoMonthlyPlanFrom_Should_ReturnException_When_DoesntHaveADemoDataForAPlan()
        {
            //Arrange
            var plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._monthlyPlanRepository.GetDemoMonthlyPlan(plan_id).Returns(new List<MonthlyPlanDemo>());

            //Act
            Func<MonthlyPlanDemo> act = () => this._monthlyPlanService.GetDemoMonthlyPlan(plan_id);

            //Assert
            var ex = Assert.Throws<Exception>(act);
            Assert.Equal("Errorrs, insert demo for plan_id not exist", ex.Message);
        }
    }
}
