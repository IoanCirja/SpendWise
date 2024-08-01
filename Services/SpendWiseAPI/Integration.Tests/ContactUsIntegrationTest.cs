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

namespace Integration.Tests
{
    public class ContactUsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ContactUsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        private async Task<HttpResponseMessage> AddContactUsAsync(ContactUsContract contactUsContract)
        {
            return await _client.PostAsJsonAsync("/ContactUs/AddSubscription", contactUsContract);
        }


        [Fact]
        public async Task AddSubscription_Should_ReturnSuccess_When_InfoIsComplete()
        {
            // Arrange
            var contactUsContract = new ContactUsContract
            {
                firstName = "Ionut",
                lastName = "Vasile",
                email = "ionut.vasile@gmail.com",
                message = "I have a problem"
            };

            // Simularea adăugării formularului de contact
            _factory.MockContactUsRepository.AddFormContactUs(Arg.Any<ContactUs>())
                .Returns(Task.FromResult(true));
            _factory.MockAuthenticationRepository.GetAdminEmail()
                .Returns(new List<string> { "admin@example.com" });

            // Act
            var response = await AddContactUsAsync(contactUsContract);

            // Assert
            response.EnsureSuccessStatusCode();
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain("Contact us form added successfully.");
        }

        [Fact]
        public async Task AddSubscription_Should_ReturnInternalServerError_When_ErrorOccurs()
        {
            // Arrange
            var contactUsContract = new ContactUsContract
            {
                firstName = "Ionut",
                lastName = "Vasile",
                email = "ionut.vasile@gmail.com",
                message = "I have a problem"
            };

            // Simularea eșecului la adăugarea formularului
            _factory.MockContactUsRepository.AddFormContactUs(Arg.Any<ContactUs>())
                .Returns(Task.FromResult(false));

            // Act
            var response = await AddContactUsAsync(contactUsContract);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var resultMessage = await response.Content.ReadAsStringAsync();
            resultMessage.Should().Contain("An error occurred while adding the contact us form.");
        }        
    }
}
