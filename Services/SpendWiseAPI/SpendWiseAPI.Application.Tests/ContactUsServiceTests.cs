using Application.Interfaces;
using Application.Services;
using NSubstitute;
using Domain;
using System.Diagnostics;
using NSubstitute.ExceptionExtensions;
using FluentAssertions;

namespace SpendWiseAPI.Application.Tests
{
    public class ContactUsServiceTests
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IEmailSender _emailSender;
        private readonly ContactUsService _contactUsService;
        public ContactUsServiceTests()
        {
            this._contactUsRepository = Substitute.For<IContactUsRepository>();
            this._authenticationRepository = Substitute.For<IAuthenticationRepository>();
            this._emailSender = Substitute.For<IEmailSender>();
            this._contactUsService = new ContactUsService(this._contactUsRepository, this._authenticationRepository, this._emailSender);
        }
        [Fact]
        public async Task AddFormContactUs_Should_ReturnException_When_DoesntExistAdmin()
        {
            //Arrange
            var contactUs = new ContactUs
            {
                firstName = "Ionut",
                lastName = "Vasile",
                message = "I have a problem",
                email = "ionut.vasile@gmail.com",
                status = "Send"
            };
            var email = new List<string>();
            string subject = "A new contact us form has been completed.";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>A new contact us form has been completed.</strong></p>
                                        <p>First Name: {contactUs.firstName}</p>
                                        <p>Last Name: {contactUs.lastName}</p>
                                        <p>Email: {contactUs.email}</p>
                                        <p>Message: {contactUs.message}</p>
                                    <body>
                                <html>
                               ";
            this._contactUsRepository.AddFormContactUs(contactUs).Returns(true);
            this._authenticationRepository.GetAdminEmail().Returns(new List<string>());
            this._emailSender.SendEmailAsync(email,subject,body).Returns(Task.CompletedTask);

            //Act
            Func<Task<bool>> act = async () => await this._contactUsService.AddFormContactUs(contactUs);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("don't have user with role admin", ex.Message);
        }
        [Fact]
        public async Task AddFormContactUs_Should_ReturnTrue_When_InfoIsComplete()
        {
            //Arrange
            var contactUs = new ContactUs
            {
                firstName = "Ionut",
                lastName = "Vasile",
                message = "I have a problem",
                email = "ionut.vasile@gmail.com",
                status = "Send"
            };
            var email = new List<string>() { "vasileion@gmail.com" };
            string subject = "A new contact us form has been completed.";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>A new contact us form has been completed.</strong></p>
                                        <p>First Name: {contactUs.firstName}</p>
                                        <p>Last Name: {contactUs.lastName}</p>
                                        <p>Email: {contactUs.email}</p>
                                        <p>Message: {contactUs.message}</p>
                                    <body>
                                <html>
                               ";
            this._contactUsRepository.AddFormContactUs(contactUs).Returns(true);
            this._authenticationRepository.GetAdminEmail().Returns(new List<string>()
            {
                "vasileion@gmail.com"
            });
            this._emailSender.SendEmailAsync(email, subject, body).Returns(Task.CompletedTask);

            //Act
            var result = await this._contactUsService.AddFormContactUs(contactUs);

            //Assert
            result.Should().BeTrue();
        }
    }
}