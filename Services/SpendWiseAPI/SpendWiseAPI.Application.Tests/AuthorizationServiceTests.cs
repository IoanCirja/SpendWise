using Application.Interfaces;
using Application.Services;
using Domain;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace SpendWiseAPI.Application.Tests
{
    public class AuthorizationServiceTests
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IIdentityHandler _identityHandler;
        private readonly AuthorizationService _authorizationService;
        public AuthorizationServiceTests()
        {
            this._passwordHasher=Substitute.For<IPasswordHasher>();
            this._authenticationRepository=Substitute.For<IAuthenticationRepository>();
            this._identityHandler=Substitute.For<IIdentityHandler>();
            this._authorizationService=new AuthorizationService(this._passwordHasher, this._authenticationRepository,this._identityHandler);
        }
        [Fact]
        public async Task RegisterUser_Should_ReturnTrue_When_InformationIsCorrect()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Name = "Ionut Vasile",
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "Password123456",
                Phone = "0761234567",
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>());
            this._passwordHasher.Hash(userCredential.Password).Returns("1234567890qwertyuiop");
            this._authenticationRepository.RegisterUser(Arg.Is<UserCredentials>(user =>  
                user.Name == userCredential.Name &&
                user.Email == userCredential.Email &&
                user.Password == "1234567890qwertyuiop" &&
                user.Phone == userCredential.Phone &&
                user.Role == "user"
            )).Returns(true);

            //Act
            var result = await this._authorizationService.RegisterUser(userCredential);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task RegisterUser_Should_ReturnException_When_PasswordsAreDiferent()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Name = "Ionut Vasile",
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "Password12345612",
                Phone = "0761234567",
                Role = "user"
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>());
            this._passwordHasher.Hash(userCredential.Password).Returns("1234567890qwertyuiop");
            this._authenticationRepository.RegisterUser(userCredential).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.RegisterUser(userCredential);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Passwords are different", ex.Message);
        }
        [Fact]
        public async Task RegisterUser_Should_ReturnException_When_UserAlreadyHasAccount()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Name = "Ionut Vasile",
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
                ConfirmPassword = "Password123456",
                Phone = "0761234567",
                Role = "user"
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>()
            { 
                userCredential
            });
            this._passwordHasher.Hash(userCredential.Password).Returns("1234567890qwertyuiop");
            this._authenticationRepository.RegisterUser(userCredential).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.RegisterUser(userCredential);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User already registered", ex.Message);
        }
        [Fact]
        public async Task LoginUser_Should_ReturnTrue_When_InformationIsCorrect()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123456",
                    Role = "user",
                    Phone = "0761234567",
                }
            });
            this._passwordHasher.Verify(userCredential.Password, "Password123456").Returns(true);
            this._authenticationRepository.RegisterUser(userCredential).Returns(true);
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile" &&
                user.Role == "user"
                )).Returns("12345678");

            //Act
            var result = await this._authorizationService.LoginUser(userCredential);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"));
            result.Name.Should().Be("Ionut Vasile");
            result.JwtToken.Should().Be("12345678");
            result.Role.Should().Be("user");
        }
        [Fact]
        public async Task LoginUser_Should_ReturnException_When_PasswordIsIncorrect()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password1234561",
                    Phone = "0761234567",
                }
            });
            this._passwordHasher.Verify(userCredential.Password, "Password123456").Returns(false);
            this._authenticationRepository.RegisterUser(userCredential).Returns(true);
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile" &&
                user.Role == "user"
                )).Returns("12345678");

            //Act
            Func<Task<User>> act = async () => await this._authorizationService.LoginUser(userCredential);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Username or password are incorrect", ex.Message);
        }
        [Fact]
        public async Task LoginUser_Should_ReturnException_When_UserDoesntHaveAccount()
        {
            //Arrange
            var userCredential = new UserCredentials
            {
                Email = "ionut.vasile@gmail.com",
                Password = "Password123456",
            };
            this._authenticationRepository.GetUser(userCredential.Email).Returns(new List<UserCredentials>());
            this._passwordHasher.Verify(userCredential.Password, "Password123456").Returns(false);
            this._authenticationRepository.RegisterUser(userCredential).Returns(true);
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile" &&
                user.Role == "user"
                )).Returns("12345678");

            //Act
            Func<Task<User>> act = async () => await this._authorizationService.LoginUser(userCredential);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Username or password are incorrect", ex.Message);
        }
        [Fact]
        public async Task GiveAdminRole_Should_ReturnTrue_When_InformationIsCorrect()
        {
            //Arrange
            string email = "ionut.vasile@gmail.com";
            this._authenticationRepository.GetUser(email).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123456",
                    Role = "user",
                    Phone = "0761234567",
                }
            });
            this._authenticationRepository.GiveUserAdminRights(email).Returns(true);

            //Act
            var result = await this._authorizationService.GiveUserAdminRights(email);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task GiveAdminRole_Should_ReturnException_When_UserDoesntExist()
        {
            //Arrange
            string email = "ionut.vasile@gmail.com";
            this._authenticationRepository.GetUser(email).Returns(new List<UserCredentials>());
            this._authenticationRepository.GiveUserAdminRights(email).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.GiveUserAdminRights(email);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User is not registered", ex.Message);
        }
        [Fact]
        public async Task SaveAccountSettings_Should_ReturnTrue_When_InformationIsCorrect()
        {
            //Arrange
            var user = new User
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                Name = "Ionut Vasile1",
                Email = "ionut.vasile1@gmail.com",
                Phone = "0761234568",
                Role = "user"
            };
            this._authenticationRepository.SaveAccountSettings(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == user.Name &&
                userF.Email == user.Email &&
                userF.Password == "123456qwerty" &&
                userF.Phone == user.Phone &&
                userF.Role == "user"
                )).Returns(true);
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "123456qwerty",
                    Phone = "0761234567",
                    Role = "user"
                }
            }, new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile1",
                    Email = "ionut.vasile1@gmail.com",
                    Password = "123456qwerty",
                    Phone = "0761234568",
                    Role = "user"
                }
            });
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile1" &&
                user.Role == "user"
                )).Returns("12345679");

            //Act
            var result = await this._authorizationService.SaveAccountSettings(user);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"));
            result.Name.Should().Be("Ionut Vasile1");
            result.JwtToken.Should().Be("12345679");
            result.Role.Should().Be("user");
        }
        [Fact]
        public async Task SaveAccountSettings_Should_ReturnException_When_UserDoesntRegistered()
        {
            //Arrange
            var user = new User
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                Name = "Ionut Vasile1",
                Email = "ionut.vasile1@gmail.com",
                Phone = "0761234568",
                Role = "user"
            };
            this._authenticationRepository.SaveAccountSettings(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == user.Name &&
                userF.Email == user.Email &&
                userF.Password == "123456qwerty" &&
                userF.Phone == user.Phone &&
                userF.Role == "user"
                )).Returns(true);
            this._authenticationRepository.GetUserByID(user.ID).Returns(
                new List<UserCredentials>(), new List<UserCredentials>()
                {
                    new UserCredentials
                    {
                        user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                        Name = "Ionut Vasile1",
                        Email = "ionut.vasile1@gmail.com",
                        Password = "123456qwerty",
                        Phone = "0761234568",
                        Role = "user"
                    }
                });
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile1" &&
                user.Role == "user"
                )).Returns("12345679");

            //Act
            Func<Task<User>> act = async () => await this._authorizationService.SaveAccountSettings(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User are not registered", ex.Message);
        }
        [Fact]
        public async Task SaveAccountSettings_Should_ReturnException_When_SavedFailed()
        {
            //Arrange
            var user = new User
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                Name = "Ionut Vasile1",
                Email = "ionut.vasile1@gmail.com",
                Phone = "0761234568",
                Role = "user"
            };
            this._authenticationRepository.SaveAccountSettings(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == user.Name &&
                userF.Email == user.Email &&
                userF.Password == "123456qwerty" &&
                userF.Phone == user.Phone &&
                userF.Role == "user"
                )).Returns(false);
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "123456qwerty",
                    Phone = "0761234567",
                    Role = "user"
                }
            }, new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile1",
                    Email = "ionut.vasile1@gmail.com",
                    Password = "123456qwerty",
                    Phone = "0761234568",
                    Role = "user"
                }
            });
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile1" &&
                user.Role == "user"
                )).Returns("12345679");

            //Act
            Func<Task<User>> act = async () => await this._authorizationService.SaveAccountSettings(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("errors: account settings saving failed", ex.Message);
        }
        [Fact]
        public async Task SaveAccountSettings_Should_ReturnException_When_UserIDIsIncorrect()
        {
            //Arrange
            var user = new User
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                Name = "Ionut Vasile1",
                Email = "ionut.vasile1@gmail.com",
                Phone = "0761234568",
                Role = "user"
            };
            this._authenticationRepository.SaveAccountSettings(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == user.Name &&
                userF.Email == user.Email &&
                userF.Password == "123456qwerty" &&
                userF.Phone == user.Phone &&
                userF.Role == "user"
                )).Returns(true);
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "123456qwerty",
                    Phone = "0761234567",
                    Role = "user"
                }
            }, new List<UserCredentials>()
            );
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile1" &&
                user.Role == "user"
                )).Returns("12345679");

            //Act
            Func<Task<User>> act = async () => await this._authorizationService.SaveAccountSettings(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("ID are incorrect", ex.Message);
        }
        [Fact]
        public async Task ResetPassword_Should_ReturnTrue_When_InformationIsCorrect()
        {
            //Arrange
            var user = new ResetPassword
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                CurrentPassword = "Password123",
                NewPassword = "qwerty123456",
                ConfirmPassword = "qwerty123456"
            };
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123",
                    Phone = "0761234567",
                    Role = "user"
                }
            });
            this._passwordHasher.Verify(user.CurrentPassword, "Password123").Returns(true);
            this._passwordHasher.Hash("qwerty123456").Returns("1234567890qwertyuiop");
            this._authenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == "Ionut Vasile" &&
                userF.Email == "ionut.vasile@gmail.com" &&
                userF.Password == "1234567890qwertyuiop" &&
                userF.Phone == "0761234567" &&
                userF.Role == "user"
                )).Returns(true);

            //Act
            var result = await this._authorizationService.ResetPassword(user);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task ResetPassword_Should_ReturnException_When_UserDoesntRegistered()
        {
            //Arrange
            var user = new ResetPassword
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                CurrentPassword = "Password123",
                NewPassword = "qwerty123456",
                ConfirmPassword = "qwerty123456"
            };
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>());
            this._passwordHasher.Verify(user.CurrentPassword, "Password123456").Returns(true);
            this._passwordHasher.Hash("qwerty123456").Returns("1234567890qwertyuiop");
            this._authenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == "Ionut Vasile" &&
                userF.Email == "ionut.vasile@gmail.com" &&
                userF.Password == "123456qwerty" &&
                userF.Phone == "0761234567" &&
                userF.Role == "user"
                )).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.ResetPassword(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User are not registered", ex.Message);
        }
        [Fact]
        public async Task ResetPassword_Should_ReturnException_When_CurrentPasswordIncorrect()
        {
            //Arrange
            var user = new ResetPassword
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                CurrentPassword = "Password123",
                NewPassword = "qwerty123456",
                ConfirmPassword = "qwerty123456"
            };
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123456",
                    Phone = "0761234567",
                    Role = "user"
                }
            });
            this._passwordHasher.Verify(user.CurrentPassword, "Password123456").Returns(false);
            this._passwordHasher.Hash("qwerty123456").Returns("1234567890qwertyuiop");
            this._authenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == "Ionut Vasile" &&
                userF.Email == "ionut.vasile@gmail.com" &&
                userF.Password == "123456qwerty" &&
                userF.Phone == "0761234567" &&
                userF.Role == "user"
                )).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.ResetPassword(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Current password is incorrect!", ex.Message);
        }
        [Fact]
        public async Task ResetPassword_Should_ReturnException_When_PasswordsDifferent()
        {
            //Arrange
            var user = new ResetPassword
            {
                ID = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                CurrentPassword = "Password123456",
                NewPassword = "qwerty1234",
                ConfirmPassword = "qwerty123456"
            };
            this._authenticationRepository.GetUserByID(user.ID).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123456",
                    Phone = "0761234567",
                    Role = "user"
                }
            });
            this._passwordHasher.Verify(user.CurrentPassword, "Password123456").Returns(true);
            this._passwordHasher.Hash("qwerty123456").Returns("1234567890qwertyuiop");
            this._authenticationRepository.ResetPassword(Arg.Is<UserCredentials>(userF =>
                userF.user_id == user.ID &&
                userF.Name == "Ionut Vasile" &&
                userF.Email == "ionut.vasile@gmail.com" &&
                userF.Password == "123456qwerty" &&
                userF.Phone == "0761234567" &&
                userF.Role == "user"
                )).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._authorizationService.ResetPassword(user);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Passwords are different", ex.Message);
        }
        [Fact]
        public async Task GetUserInfo_Should_ReturnException_When_InformationIsCorrect()
        {
            //Arrange
            var user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._authenticationRepository.GetUserByID(user_id).Returns(new List<UserCredentials>()
            {
                new UserCredentials
                {
                    user_id =  Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    Name = "Ionut Vasile",
                    Email = "ionut.vasile@gmail.com",
                    Password = "Password123456",
                    Phone = "0761234567",
                    Role = "user"
                }
            });
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile" &&
                user.Role == "user"
                )).Returns("12345679");

            //Act
            var result = await this._authorizationService.GetUserInfo(user_id);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"));
            result.Name.Should().Be("Ionut Vasile");
            result.JwtToken.Should().Be("12345679");
            result.Role.Should().Be("user");
        }
        [Fact]
        public async Task GetUserInfo_Should_ReturnException_When_userDoesntRegistered()
        {
            //Arrange
            var user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");
            this._authenticationRepository.GetUserByID(user_id).Returns(new List<UserCredentials>());
            this._identityHandler.GenerateToken(Arg.Is<User>(user =>
                user.ID == Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7") &&
                user.Name == "Ionut Vasile1" &&
                user.Role == "user"
                )).Returns("12345679");


            //Act
            Func<Task<User>> act = async () => await this._authorizationService.GetUserInfo(user_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("User are not registered", ex.Message);
        }
    }
}
