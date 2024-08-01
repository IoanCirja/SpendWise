using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using Integration.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApi;
using WebApiContracts;
using Xunit;

namespace Integration.Tests
{
    public class MonthlyPlanControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;


        public MonthlyPlanControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task AddMonthlyPlans_Should_ReturnSuccess_When_InfoIsComplete()
        {
            // Arrange
            var monthlyPlanContract = new MonthlyPlanContract
            {
                user_id = Guid.NewGuid(),
                plan_id = Guid.NewGuid(),
                date = DateTime.UtcNow,
                totalAmount = 1000,
                amountSpent = 200,
                priceByCategory = "Food:500,Transport:300,Entertainment:200",
                spentOfCategory = "Food:150,Transport:50,Entertainment:0"
            };

            _factory.MockMonthlyPlanRepository.AddMonthlyPlans(Arg.Any<MonthlyPlan>())
                .Returns(Task.FromResult(true));

            var client = _factory.CreateClient();
            var content = JsonContent.Create(monthlyPlanContract);

            // Act
            var response = await client.PostAsync("/MonthlyPlan/AddMonthlyPlans", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain("Monthly plan added successfully.");
        }

        [Fact]
        public async Task AddMonthlyPlans_Should_Return500_When_UserAlreadyHasActivePlan()
        {
            // Arrange
            var monthlyPlanContract = new MonthlyPlanContract
            {
                user_id = Guid.NewGuid(),
                plan_id = Guid.NewGuid(),
                date = DateTime.Now,
                totalAmount = 1000,
                amountSpent = 0,
                priceByCategory = "Category1,Category2",
                spentOfCategory = "0,0"
            };

            _factory.MockMonthlyPlanRepository.VerifyUserHasPlanActive(monthlyPlanContract.user_id).Returns(true);

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/MonthlyPlan/AddMonthlyPlans", monthlyPlanContract);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain(" User already have a current plan active");
        }

        [Fact]
        public async Task CancelMonthlyPlans_Should_ReturnSuccess_When_CancellationIsSuccessful()
        {
            // Arrange
            var planId = Guid.NewGuid();
            _factory.MockMonthlyPlanRepository.CancelMonthlyPlan(planId).Returns(Task.FromResult(true));
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"/MonthlyPlan/CancelMonthlyPlans", planId);

            // Assert
            response.EnsureSuccessStatusCode();
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain("Monthly plan canceled successfully.");
        }

        [Fact]
        public async Task CancelMonthlyPlans_Should_Return500_When_CancellationFails()
        {
            // Arrange
            var planId = Guid.NewGuid();
            _factory.MockMonthlyPlanRepository.CancelMonthlyPlan(planId).Returns(Task.FromResult(false));
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"/MonthlyPlan/CancelMonthlyPlans", planId);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain("An error occurred while adding the monthly plan.");
        }

        [Fact]
        public async Task GetPlanFromHistory_Should_ReturnPlan_When_PlanExists()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = new List<MonthlyPlanGet>
            {
                new MonthlyPlanGet {  }
            };
            _factory.MockMonthlyPlanRepository.GetMonthlyPlanFromHistory(planId).Returns(plan);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/MonthlyPlan/GetPlanFromHistory/{planId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<MonthlyPlanGet>>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(plan);
        }

        [Fact]
        public async Task GetDemoMonthlyPlan_Should_ReturnDemoPlan_When_PlanExists()
        {
            // Arrange
            var demoPlanId = Guid.NewGuid();
            var demoPlan = new MonthlyPlanDemo { /* Initialize properties */ };
            _factory.MockMonthlyPlanRepository.GetDemoMonthlyPlan(demoPlanId).Returns(new List<MonthlyPlanDemo> { demoPlan });
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/MonthlyPlan/GetDemoMonthlyPlan/{demoPlanId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<MonthlyPlanDemo>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(demoPlan);
        }

        [Fact]
        public async Task GetHistoryPlans_Should_ReturnListOfPlans_When_UserExists()
        {
            // Arrange
            var user_id = Guid.NewGuid();
            var expectedPlans = new List<MonthlyPlanGetNameDate>
    {
        new MonthlyPlanGetNameDate { plan_name = "Test Plan 1", date = DateTime.Now },
        new MonthlyPlanGetNameDate { plan_name = "Test Plan 2", date = DateTime.Now.AddDays(-1) }
    };

            // Mock service method
            _factory.MockMonthlyPlanRepository.GetHistoryPlans(user_id).Returns(expectedPlans);

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/MonthlyPlan/GetHistoryPlans/{user_id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<MonthlyPlanGetNameDate>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedPlans);
        }


        [Fact]
        public async Task GetCurrentPlan_Should_ReturnCurrentPlan_When_UserHasOnePlan()
        {
            // Arrange
            var user_id = Guid.NewGuid();
            var monthlyPlan = new MonthlyPlanGet
            {
                monthlyPlan_id = Guid.NewGuid(),
                user_id = user_id,
                date = DateTime.Now,
            };

            // Mock repository methods
            _factory.MockMonthlyPlanRepository.GetDateFromMonthlyPlanByUserID(user_id)
                .Returns(new List<MonthlyPlanGetDateID> { new MonthlyPlanGetDateID { monthlyPlan_id = monthlyPlan.monthlyPlan_id, date = monthlyPlan.date } });

            _factory.MockMonthlyPlanRepository.GetCurrentPlan(user_id)
                .Returns(new List<MonthlyPlanGet> { monthlyPlan });

            // Act
            var result = _factory.MockMonthlyPlanRepository.GetCurrentPlan(user_id);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.First().monthlyPlan_id.Should().Be(monthlyPlan.monthlyPlan_id);
        }

        [Fact]
        public async Task ExportDetailsByMonthAndYearToPdf_Should_ReturnBadRequest_When_ExceptionThrown()
        {
            // Arrange
            var user_id = Guid.NewGuid();
            var year = 2024;
            var month = 8;

            // Mock the service method to throw an exception
            _factory.MockMonthlyPlanService.ExportCurrentDetailsToPdf(user_id, year, month)
                .Throws(new Exception("An error occurred while generating the PDF."));

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/MonthlyPlan/ExportDetailsByMonthAndYearToPdf/{user_id}/{year}/{month}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
