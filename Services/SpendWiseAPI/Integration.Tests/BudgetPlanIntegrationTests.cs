using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Integration.Tests.Setup;
using Domain;
using WebApi;
using WebApiContracts;
using NSubstitute;
using WebApiContracts.Mappers;
using System.Numerics;

namespace Integration.Tests
{
    public class BudgetPlanControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BudgetPlanControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        private async Task<HttpResponseMessage> AddPlanAsync(BudgetPlanContract planContract)
        {
            return await _client.PostAsJsonAsync("/BudgetPlan/AddPlan", planContract);
        }

        private async Task<HttpResponseMessage> RegisterUserAsync(UserCredentials userCredential)
        {
            return await _client.PostAsJsonAsync("/Authentication/RegisterUser", userCredential);
        }

        [Fact]
        public async Task GetPlans_Should_ReturnSuccess()
        {
            // Arrange
            var expectedPlans = new List<BudgetPlanGet>
            {
                new BudgetPlanGet
                {
                    plan_id = Guid.NewGuid(),
                    name = "Plan 1",
                    description = "Description 1",
                    noCategory = 2,
                    category = "Category 1",
                    image = "image1.jpg",
                    created_by = "User 1",
                    isActive = true,
                    creationDate = DateTime.Today
                },
                new BudgetPlanGet
                {
                    plan_id = Guid.NewGuid(),
                    name = "Plan 2",
                    description = "Description 2",
                    noCategory = 3,
                    category = "Category 2",
                    image = "image2.jpg",
                    created_by = "User 2",
                    isActive = true,
                    creationDate = DateTime.Today
                }
            };

            _factory.MockBudgetPlanRepository.GetPlans().Returns(expectedPlans);

            // Act
            var response = await _client.GetAsync("/BudgetPlan/GetPlans");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<BudgetPlanGet>>();
            result.Should().BeEquivalentTo(expectedPlans);
        }

        [Fact]
        public async Task GetPlan_Should_ReturnSuccess_When_PlanExists()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var expectedPlan = new BudgetPlanGet
            {
                plan_id = planId,
                name = "Plan 1",
                description = "Description 1",
                noCategory = 2,
                category = "Category 1",
                image = "image1.jpg",
                created_by = "User 1",
                isActive = true,
                creationDate = DateTime.Today
            };

            _factory.MockBudgetPlanRepository.GetPlan(planId).Returns(new List<BudgetPlanGet> { expectedPlan });

            // Act
            var response = await _client.GetAsync($"/BudgetPlan/GetPlan/{planId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<BudgetPlanGet>>();
            result.Count.Should().Be(1);
            result[0].name.Should().Be(expectedPlan.name);
        }


        [Fact]
        public async Task AddPlan_Should_ReturnSuccess_When_PlanIsValid()
        {
            // Arrange
            var userCredential = new UserCredentials
            {
                Name = "Alex Popescu",
                Email = "alex.popescu99@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "Password123456",
                Phone = "0761234568",
                user_id = Guid.NewGuid(),
                Role = "user"
            };

            // Act
            var response1 = await RegisterUserAsync(userCredential);

            var newPlan = new BudgetPlanContract
            {
                Name = "New Plan",
                Description = "New Description",
                NoCategory = 2,
                Category = "New Category",
                Imagine = "new_image.jpg",
                user_id = userCredential.user_id
            };

            // Mock the add plan process
            _factory.MockBudgetPlanRepository.AddPlan(newPlan.MapTestToDomain()).Returns(Task.FromResult(true));

            // Act
            var response = await AddPlanAsync(newPlan);

            // Assert
            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task EditPlanByPlanId_Should_ReturnSuccess_WhenPlanIsValid()
        {
            // Arrange
            var existingPlan = new BudgetPlan
            {
                plan_id = Guid.NewGuid(),
                name = "Existing Budget Plan",
                description = "This is a description for the existing budget plan.",
                noCategory = 3,
                category = "Savings, Investment, Expenses",
                image = "image_url.jpg",
                isActive = true,
                creationDate = DateTime.UtcNow,
                created_by = Guid.NewGuid()
            };

            var updatedPlanContract = new BudgetPlanEditContract
            {
                Name = "Updated Budget Plan",
                Description = "This is a description for the updated budget plan.",
                NoCategory = 4,
                Category = "Savings, Investment, Expenses, Entertainment",
                Image = "updated_image_url.jpg",
            };

            var updatedPlan = new BudgetPlan
            {
                plan_id = existingPlan.plan_id,
                name = updatedPlanContract.Name,
                description = updatedPlanContract.Description,
                noCategory = updatedPlanContract.NoCategory,
                category = updatedPlanContract.Category,
                image = updatedPlanContract.Image,
                isActive = true,
                creationDate = existingPlan.creationDate,
                created_by = existingPlan.created_by
            };

            _factory.MockBudgetPlanRepository.AddPlan(existingPlan).Returns(Task.FromResult(true));

            // Simularea căutării planului înainte de actualizare
            _factory.MockBudgetPlanRepository.GetPlanById(existingPlan.plan_id).Returns(Task.FromResult(existingPlan));
            _factory.MockBudgetPlanRepository.GetPlanByName(updatedPlanContract.Name).Returns(Task.FromResult<BudgetPlan>(null)); // Nu există alt plan cu acest nume

            // Simularea actualizării planului
            _factory.MockBudgetPlanRepository.EditPlanById(Arg.Any<BudgetPlan>(), existingPlan.plan_id).Returns(Task.FromResult(updatedPlan));

            // Act
            var editResponse = await _client.PostAsJsonAsync($"/BudgetPlan/EditPlanByPlanId/{existingPlan.plan_id}", updatedPlanContract);

            // Assert
            editResponse.EnsureSuccessStatusCode(); // Asigură-te că cererea a avut succes
            var result = await editResponse.Content.ReadFromJsonAsync<BudgetPlan>(); // Utilizează metoda corectă
            result.Should().NotBeNull();
            result.name.Should().Be("Updated Budget Plan");
            result.description.Should().Be("This is a description for the updated budget plan.");
            result.noCategory.Should().Be(4);
            result.category.Should().Be("Savings, Investment, Expenses, Entertainment");
            result.image.Should().Be("updated_image_url.jpg");
            result.isActive.Should().BeTrue();
        }


        [Fact]
        public async Task EditPlanByPlanName_Should_ReturnSuccess_WhenPlanIsValid()
        {
            // Arrange
            var existingPlan = new BudgetPlan
            {
                plan_id = Guid.NewGuid(),
                name = "Existing Budget Plan",
                description = "This is a description for the existing budget plan.",
                noCategory = 3,
                category = "Savings, Investment, Expenses",
                image = "image_url.jpg",
                isActive = true,
                creationDate = DateTime.UtcNow,
                created_by = Guid.NewGuid()
            };

            var updatedPlanContract = new BudgetPlanEditContract
            {
                Name = "Updated Budget Plan",
                Description = "This is a description for the updated budget plan.",
                NoCategory = 4,
                Category = "Savings, Investment, Expenses, Entertainment",
                Image = "updated_image_url.jpg",
            };

            var updatedPlan = new BudgetPlan
            {
                plan_id = existingPlan.plan_id,
                name = updatedPlanContract.Name,
                description = updatedPlanContract.Description,
                noCategory = updatedPlanContract.NoCategory,
                category = updatedPlanContract.Category,
                image = updatedPlanContract.Image,
                isActive = true,
                creationDate = existingPlan.creationDate,
                created_by = existingPlan.created_by
            };

            // Simularea adăugării planului
            _factory.MockBudgetPlanRepository.AddPlan(existingPlan).Returns(Task.FromResult(true));

            // Simularea căutării planului înainte de actualizare
            _factory.MockBudgetPlanRepository.GetPlanByName(existingPlan.name).Returns(Task.FromResult(existingPlan));
            _factory.MockBudgetPlanRepository.GetPlanByName(updatedPlanContract.Name).Returns(Task.FromResult<BudgetPlan>(null)); // Nu există alt plan cu acest nume

            // Simularea actualizării planului
            _factory.MockBudgetPlanRepository.EditPlanByName(Arg.Any<BudgetPlan>(), existingPlan.name).Returns(Task.FromResult(updatedPlan));


            // Act
            var editResponse = await _client.PostAsJsonAsync($"/BudgetPlan/EditPlanMyName/{existingPlan.name}", updatedPlanContract);

            // Assert
            editResponse.EnsureSuccessStatusCode(); // Asigură-te că cererea a avut succes
            var result = await editResponse.Content.ReadFromJsonAsync<BudgetPlan>(); // Utilizează metoda corectă
            result.Should().NotBeNull();
            result.name.Should().Be("Updated Budget Plan");
            result.description.Should().Be("This is a description for the updated budget plan.");
            result.noCategory.Should().Be(4);
            result.category.Should().Be("Savings, Investment, Expenses, Entertainment");
            result.image.Should().Be("updated_image_url.jpg");
            result.isActive.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePlanByPlanName_Should_ReturnSuccess_WhenPlanIsValid()
        {
            // Arrange
            var newPlan = new BudgetPlan
            {
                plan_id = Guid.NewGuid(),
                name = "New Budget Plan",
                description = "This is a description for the new budget plan.",
                noCategory = 3,
                category = "Savings, Investment, Expenses",
                image = "image_url.jpg",
                isActive = true,
                creationDate = DateTime.UtcNow,
                created_by = Guid.NewGuid()
            };

            // Simularea adăugării planului
            _factory.MockBudgetPlanRepository.AddPlan(newPlan).Returns(Task.FromResult(true));

            // Simularea căutării planului înainte de ștergere
            _factory.MockBudgetPlanRepository.GetPlanByName(newPlan.name)
                .Returns(Task.FromResult(newPlan)); // Returnează planul existent

            // Simularea ștergerii planului
            _factory.MockBudgetPlanRepository.DeletePlanByName(newPlan.name)
                .Returns(Task.FromResult($"Plan with name '{newPlan.name}' was deleted")); // Simulează ștergerea

            // Act
            var response = await _client.DeleteAsync($"/BudgetPlan/DeletePlanByName/{newPlan.name}");

            // Assert
            response.EnsureSuccessStatusCode(); // Asigură-te că cererea a avut succes
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain($"Plan with name '{newPlan.name}' was deleted");
        }

        [Fact]
        public async Task DeletePlanById_Should_ReturnInternalServerError_When_PlanDoesNotExist()
        {
            // Arrange
            var planId = Guid.NewGuid();
            _factory.MockBudgetPlanRepository.DeletePlanById(planId).Returns(Task.FromResult("Plan with the specified id does not exist"));

            // Act
            var response = await _client.DeleteAsync($"/BudgetPlan/DeletePlanById/{planId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("Some generic error message: System.Exception: Plan not found");
        }


        [Fact]
        public async Task GetPlansByAdminCreator_Should_ReturnPlans_WhenAdminIdIsValid()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var plans = new List<BudgetPlan>
        {
        new BudgetPlan
        {
            plan_id = Guid.NewGuid(),
            name = "Plan 1",
            description = "Description for Plan 1",
            noCategory = 3,
            category = "Savings, Investment, Expenses",
            image = "image_url_1.jpg",
            isActive = true,
            creationDate = DateTime.UtcNow,
            created_by = adminId
        },
        new BudgetPlan
        {
            plan_id = Guid.NewGuid(),
            name = "Plan 2",
            description = "Description for Plan 2",
            noCategory = 2,
            category = "Savings, Investment",
            image = "image_url_2.jpg",
            isActive = true,
            creationDate = DateTime.UtcNow,
            created_by = adminId
        }
    };

            // Simularea returnării planurilor pentru un anumit admin
            _factory.MockBudgetPlanRepository.GetPlansByAdminCreator(adminId)
                    .Returns(plans);


            // Act
            var response = await _client.GetAsync($"/BudgetPlan/GetPlansByAdminCreator/{adminId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Asigură-te că cererea a avut succes
            var result = await response.Content.ReadFromJsonAsync<List<BudgetPlan>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().name.Should().Be("Plan 1");
            result.Last().name.Should().Be("Plan 2");
        }

        [Fact]
        public async Task GetPopularFivePlans_Should_ReturnTopFivePlans()
        {
            // Arrange
            var popularPlans = new List<BudgetPlanGetPopular>
{
    new BudgetPlanGetPopular
    {
        plan_id = Guid.NewGuid(),
        name = "Popular Plan 1",
        description = "Description for Popular Plan 1",
        noCategory = 3,
        category = "Savings, Investment, Expenses",
        image = "image_url_1.jpg",
        numberOfUse = 100 // Exemplu de număr de utilizări
    },
    new BudgetPlanGetPopular
    {
        plan_id = Guid.NewGuid(),
        name = "Popular Plan 2",
        description = "Description for Popular Plan 2",
        noCategory = 2,
        category = "Savings, Investment",
        image = "image_url_2.jpg",
        numberOfUse = 80 // Exemplu de număr de utilizări
    },
    new BudgetPlanGetPopular
    {
        plan_id = Guid.NewGuid(),
        name = "Popular Plan 3",
        description = "Description for Popular Plan 3",
        noCategory = 3,
        category = "Savings, Investment, Expenses",
        image = "image_url_3.jpg",
        numberOfUse = 60 // Exemplu de număr de utilizări
    },
    new BudgetPlanGetPopular
    {
        plan_id = Guid.NewGuid(),
        name = "Popular Plan 4",
        description = "Description for Popular Plan 4",
        noCategory = 2,
        category = "Savings, Investment",
        image = "image_url_4.jpg",
        numberOfUse = 40 // Exemplu de număr de utilizări
    },
    new BudgetPlanGetPopular
    {
        plan_id = Guid.NewGuid(),
        name = "Popular Plan 5",
        description = "Description for Popular Plan 5",
        noCategory = 1,
        category = "Savings",
        image = "image_url_5.jpg",
        numberOfUse = 20 // Exemplu de număr de utilizări
    }
};

            // Simularea returnării celor mai populare cinci planuri
            _factory.MockBudgetPlanRepository.GetPopularFivePlans()
                    .Returns(popularPlans);

            // Act
            var response = await _client.GetAsync($"BudgetPlan/GetPopularFivePlans");

            // Assert
            response.EnsureSuccessStatusCode(); // Asigură-te că cererea a avut succes
            var result = await response.Content.ReadFromJsonAsync<List<BudgetPlanGetPopular>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.First().name.Should().Be("Popular Plan 1");
        }

    }
}
