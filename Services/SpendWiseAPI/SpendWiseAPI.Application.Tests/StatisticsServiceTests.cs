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

namespace SpendWiseAPI.Application.Tests
{
    public class StatisticsServiceTests
    {
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IMonthlyPlanRepository _monthlyPlanRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IParserData _parserData;
        private readonly StatisticsService _statisticsService;
        public StatisticsServiceTests()
        {
            this._budgetPlanRepository=Substitute.For<IBudgetPlanRepository>();
            this._monthlyPlanRepository=Substitute.For<IMonthlyPlanRepository>();
            this._transactionsRepository=Substitute.For<ITransactionsRepository>();
            this._parserData=Substitute.For<IParserData>();
            this._statisticsService=new StatisticsService(this._budgetPlanRepository,this._monthlyPlanRepository,this._transactionsRepository,this._parserData);
        }
        [Fact]
        public async Task GetStatistics_Should_ReturnStatistics_When_InfoIsCorrect()
        {
            //Arrange
            var user_id = Guid.Parse("A13DA2E5-3A0E-949C-9BD9-ED3743FDA0C8");
            var budgetPlan = new BudgetPlanGetPopular
            {
                plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Family Plan",
                description = "Description",
                noCategory = 4,
                category = "Food,Health,Travel,Home",
                image = "/asserts/familyPlan.svg",
                numberOfUse = 4
            };
            var transaction = new TransactionsInfo
            {
                name = "Pizza",
                amount = 100
            };
            var transaction1 = new TransactionsInfo
            {
                name = "Diesel",
                amount = 350
            };
            var monthlyPlan = new MonthlyPlanGet
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                user_id = Guid.Parse("A13DA2E5-3A0E-949C-9BD9-ED3743FDA0C8"),
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
            this._budgetPlanRepository.GetMostUsedPlan(user_id).Returns(new List<BudgetPlanGetPopular>()
            {
                budgetPlan
            });
            this._transactionsRepository.GetBiggestTransaction(user_id).Returns(new List<TransactionsInfo>()
            {
                transaction1
            });
            this._transactionsRepository.GetSmallestTransaction(user_id).Returns(new List<TransactionsInfo>()
            {
                transaction
            });
            this._transactionsRepository.GetBiggestTransactionCurrentPlan(user_id).Returns(new List<TransactionsInfo>()
            {
                transaction1
            });
            this._transactionsRepository.GetSmallestTransactionCurrentPlan(user_id).Returns(new List<TransactionsInfo>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetCurrentPlan(user_id).Returns(new List<MonthlyPlanGet>()
            {
                monthlyPlan
            });
            this._parserData.GetCategory(monthlyPlan.category).Returns(new string[] { "Food", "Travel", "Health", "Pet", "School", "Rent" });
            this._parserData.GetPrice(monthlyPlan.priceByCategory).Returns(new double[] { 500, 750, 750, 550, 450, 500 });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "Food", 500).Returns(new List<DateTime>() { DateTime.Parse("2024-07-10 12:00:00") });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "Travel", 750).Returns(new List<DateTime>() { DateTime.Parse("2024-07-08 12:00:00") });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "Health", 750).Returns(new List<DateTime>() { DateTime.Parse("2500-10-10 00:00:00") });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "Pet", 550).Returns(new List<DateTime>() { DateTime.Parse("2500-10-10 00:00:00") });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "School", 450).Returns(new List<DateTime>() { DateTime.Parse("2500-10-10 00:00:00") });
            this._transactionsRepository.GetDate(monthlyPlan.monthlyPlan_id, "Rent", 500).Returns(new List<DateTime>() { DateTime.Parse("2500-10-10 00:00:00") });

            //Act
            var result = this._statisticsService.GetStatistics(user_id);

            //Assert
            result.biggestTransactionNameForever.Should().Be("Diesel");
            result.biggestTransactionAmountForever.Should().Be(350);

        }
    }
}
