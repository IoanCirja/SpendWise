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
    public class NewsletterServiceTests
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly IEmailSender _emailSender;
        private readonly NewsLetterService _newsletterService;
        public NewsletterServiceTests()
        {
            this._newsletterRepository = Substitute.For<INewsletterRepository>();
            this._emailSender = Substitute.For<IEmailSender>();
            this._newsletterService = new NewsLetterService(this._newsletterRepository,this._emailSender);
        }
        [Fact]
        public async Task AddSubscription_Should_ReturnException_When_AlreadySubscribed()
        {
            //Arrange
            string email = "ionut.vasile@gmail.com";
            var emails = new List<string>();
            string subject = "SpendWise";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>You have just subscribed to the SpendWise website newsletter.</strong></p>
                                    <body>
                                <html>
                               ";
            this._newsletterRepository.GetSubscriberByEmail(email).Returns(new List<Newsletter>()
            {
                new Newsletter()
                {
                    subscription_id = Guid.Parse("6F6BB909-AB72-4671-A002-DC627693FBDB"),
                    email = "ionut.vasile@gmail.com"
                }
            });
            this._newsletterRepository.AddSubscription(email).Returns(false);
            this._emailSender.SendEmailAsync(emails, subject, body).Returns(Task.CompletedTask);

            //Act
            Func<Task<bool>> act = async () => await this._newsletterService.AddSubscription(email);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("You already subscribed this page.", ex.Message);
        }
        [Fact]
        public async Task AddSubscription_Should_ReturnTrue_When_InfoIsComplete()
        {
            //Arrange
            string email = "ionut.vasile@gmail.com";
            var emails = new List<string>();
            string subject = "SpendWise";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>You have just subscribed to the SpendWise website newsletter.</strong></p>
                                    <body>
                                <html>
                               ";
            this._newsletterRepository.GetSubscriberByEmail(email).Returns(new List<Newsletter>());
            this._newsletterRepository.AddSubscription(email).Returns(true);
            this._emailSender.SendEmailAsync(emails, subject, body).Returns(Task.CompletedTask);

            //Act
            var result = await this._newsletterService.AddSubscription(email);

            //Assert
            result.Should().BeTrue();
        }
    }
}
