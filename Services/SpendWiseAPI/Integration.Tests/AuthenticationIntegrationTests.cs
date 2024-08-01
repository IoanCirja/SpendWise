using System.Net.Http.Json;
using FluentAssertions;
using WebApi;
using WebApiContracts;
using Integration.Tests.Setup;
using Domain;
using System.Net;
using NSubstitute;

namespace Integration.Tests
{
        public class AuthenticationIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
        {
            private readonly HttpClient _client;
            private readonly CustomWebApplicationFactory<Program> _factory;

            public AuthenticationIntegrationTests(CustomWebApplicationFactory<Program> factory)
            {
                _factory = factory;
                _client = factory.CreateClient();
            }

            private async Task<HttpResponseMessage> RegisterUserAsync(UserCredentials userCredential)
        {
            return await _client.PostAsJsonAsync("/Authentication/RegisterUser", userCredential);
        }

        private async Task<HttpResponseMessage> LoginUserAsync(UserCredentials userCredential)
        {
            return await _client.PostAsJsonAsync("/Authentication/LoginUser", userCredential);
        }

        private async Task<HttpResponseMessage> ResetPasswordAsync(PasswordReset passwordReset)
        {
            return await _client.PostAsJsonAsync("/Authentication/ResetPassword", passwordReset);
        }

        [Fact]
        public async Task RegisterUser_Should_ReturnSuccess_When_RegistrationIsValid()
        {
            // Arrange
            var userCredential = new UserCredentials
            {
                Name = "Alex Popescu",
                Email = "alex.popescu99@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "Password123456",
                Phone = "0761234568",
            };

            // Act
            var response = await RegisterUserAsync(userCredential);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = !await response.Content.ReadFromJsonAsync<bool>();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterUser_Should_ReturnInternalServerError_When_PasswordsDontMatch()
        {
            // Arrange
            var userCredential = new UserCredentials
            {
                Name = "Alex Popescu",
                Email = "alex.popescu1@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "DifferentPassword",
                Phone = "0761234567",
            };

            // Act
            var response = await RegisterUserAsync(userCredential);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("Passwords are different");
        }

        [Fact]
        public async Task RegisterAndLoginUser_Should_ReturnSuccess_When_RegistrationIsValid()
        {
            // Arrange
            var userCredential = new UserCredentials
            {
                Email = "alex.popescu@gmail.com",
                Password = "Password123456",
            };

            var expectedUser = new UserCredentials
            {
                user_id = Guid.NewGuid(),
                Name = "Alex Popescu",
                Email = userCredential.Email,
                Password = userCredential.Password,
                Role = "user",
                Phone = "0761234567",
            };

            _factory.MockAuthenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials> { expectedUser });
            _factory.MockPasswordHasher.Verify(userCredential.Password, userCredential.Password).Returns(true);

            // Act
            var response = await LoginUserAsync(userCredential);
            response.EnsureSuccessStatusCode();

            var loginResult = await response.Content.ReadFromJsonAsync<User>();

            // Assert
            loginResult.Should().NotBeNull();
            loginResult.Name.Should().Be(expectedUser.Name);
        }

        [Fact]
        public async Task GiveUserAdminRights_Should_ReturnUnauthorized_When_EmailIsValid()
        {
            // Arrange
            string email = "alex.popescu@gmail.com";
            _factory.MockAuthenticationRepository.GetUser(email).Returns(new List<UserCredentials> { new UserCredentials { Email = email } });

            // Act
            var response = await _client.PostAsJsonAsync("/Authentication/GiveUserAdminRights", email);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task SaveAccountSettings_Should_ReturnUser_When_AccountSettingsAreValid()
        {
            // Arrange
            var userCredentials = new User
            {
                ID = Guid.NewGuid(),
                Name = "Alex Popescu",
                Email = "alex.popescu@gmail.com",
                Phone = "0761234567"
            };

            _factory.MockAuthenticationRepository.GetUserByID(userCredentials.ID).Returns(new List<UserCredentials> { new UserCredentials { user_id = userCredentials.ID } });
            _factory.MockAuthenticationRepository.SaveAccountSettings(Arg.Any<UserCredentials>()).Returns(Task.FromResult(true));

            // Act
            var response = await _client.PostAsJsonAsync("/Authentication/SaveAccountSettings", userCredentials);
            response.EnsureSuccessStatusCode();

            var controllerResult = await response.Content.ReadAsStringAsync();
            controllerResult.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ResetPassword_Should_ReturnOk_When_PasswordResetIsSuccessful()
        {
            // Arrange
            var user = new PasswordReset
            {
                UserId = "C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7",
                CurrentPassword = "Password123",
                NewPassword = "qwerty123456",
                ConfirmNewPassword = "qwerty123456"
            };

            _factory.MockAuthenticationRepository.GetUserByID(Guid.Parse(user.UserId)).Returns(new List<UserCredentials> { new UserCredentials { Password = user.CurrentPassword } });
            _factory.MockPasswordHasher.Verify(user.CurrentPassword, "Password123").Returns(true);
            _factory.MockPasswordHasher.Hash(user.NewPassword).Returns("hashedNewPassword");
            _factory.MockAuthenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF => userF.Password == "hashedNewPassword")).Returns(true);

            // Act
            var response = await ResetPasswordAsync(user);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be("Password successfully reset");
        }

        [Fact]
        public async Task ResetPassword_Should_ReturnInternalServerError_When_PasswordResetFails()
        {
            // Arrange
            var user = new PasswordReset
            {
                UserId = "C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7",
                CurrentPassword = "Password123",
                NewPassword = "qwerty123456",
                ConfirmNewPassword = "qwerty123456"
            };

            _factory.MockAuthenticationRepository.GetUserByID(Guid.Parse(user.UserId)).Returns(new List<UserCredentials> { new UserCredentials { Password = user.CurrentPassword } });
            _factory.MockPasswordHasher.Verify(user.CurrentPassword, "Password123").Returns(true);

            // Simularea eșecului la resetarea parolei
            _factory.MockAuthenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF => userF.Password == "hashedNewPassword")).Returns(false);

            // Act
            var response = await ResetPasswordAsync(user);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("An error occured while reseting password.");
        }

        [Fact]
        public async Task GetUserInfo_Should_ReturnUserInfo_When_UserExists()
        {
            // Arrange
            var expectedUser = new UserCredentials
            {
                user_id = Guid.Parse("0578c88d-8679-4c5a-a60e-0a3eaa427378"),
                Name = "Alex Popescu",
                Email = "alex.popescu@gmail.com",
                Phone = "0761234567",
                Role = "user"
            };

            _factory.MockAuthenticationRepository.GetUserByID(expectedUser.user_id).Returns(new List<UserCredentials> { expectedUser });

            // Act
            var response = await _client.GetAsync($"/Authentication/SaveAccountSettings/{expectedUser.user_id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserCredentials>();
            result.Email.Should().Be(expectedUser.Email);
        }
    }
}
