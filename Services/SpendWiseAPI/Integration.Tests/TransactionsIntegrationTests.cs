using FluentAssertions;
using Integration.Tests.Setup;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApi;
using WebApiContracts;
using Domain;
using WebApiContracts.Mappers;

namespace Integration.Tests
{
    public class TransactionsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TransactionsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddTransaction_Should_ReturnError_When_TransactionIsAddedUnsuccessfully()
        {
            // Arrange
            var transaction = new TransactionsContract
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 0
            };

           

            // Act
            var response = await _client.PostAsJsonAsync("/Transactions/AddTransaction", transaction);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Value cannot");
        }

        [Fact]
        public async Task DeleteTransaction_Should_ReturnError_When_TransactionIsDeletedUnsuccessfully()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var transaction = new TransactionsContract
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 10
            };

            _factory.MockTransactionsRepository.AddTransaction(transaction.MapTestToDomain()).Returns(true);
            _factory.MockTransactionsRepository.DeleteTransactions(transactionId).Returns(true);

        

            var response = await _client.DeleteAsync($"/Transactions/DeleteTransactions/{transactionId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var content = await response.Content.ReadAsStringAsync();

        }

        [Fact]
        public async Task GetAllTransactions_Should_ReturnTransactions_When_MonthlyPlanIdIsValid()
        {
            // Arrange
            var monthlyPlanId = Guid.NewGuid();
            var transactions = new List<Domain.Transactions>
            {
                new Domain.Transactions { /* Setează proprietățile necesare */ }
            };

            _factory.MockTransactionsRepository.GetAllTransactions(monthlyPlanId).Returns(transactions);

            // Act
            var response = await _client.GetAsync($"/Transactions/GetAllTransactions/{monthlyPlanId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<Domain.Transactions>>();
            result.Should().BeEquivalentTo(transactions);
        }



        [Fact]
        public async Task GetTransactionForCategory_Should_ReturnTransactions_When_CategoryIsValid()
        {
            // Arrange
            var category = "Food";
            var transactions = new List<Domain.Transactions>
            {
                new Domain.Transactions { /* Setează proprietățile necesare */ }
            };

            _factory.MockTransactionsRepository.GetTransactionsForCategory(category).Returns(transactions);

            // Act
            var response = await _client.GetAsync($"/Transactions/GetTransactionForCategory/{category}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<Domain.Transactions>>();
            result.Should().BeEquivalentTo(transactions);
        }

        [Fact]
        public async Task GetAllTransactionForUser_Should_ReturnTransactions_When_UserIdIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var transactions = new List<Domain.Transactions>
            {
                new Domain.Transactions { /* Setează proprietățile necesare */ }
            };

            _factory.MockTransactionsRepository.GetAllTransactionsForUser(userId).Returns(transactions);

            // Act
            var response = await _client.GetAsync($"/Transactions/GetAllTransactionForUser/{userId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<Domain.Transactions>>();
            result.Should().BeEquivalentTo(transactions);
        }
    }
}
