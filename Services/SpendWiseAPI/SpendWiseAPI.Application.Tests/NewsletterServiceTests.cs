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
            string subject = "Welcome to the SpendWise Community!";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>Dear Subscriber,</strong></p>
                                        <p>Thank you for joining the SpendWise community! We are thrilled to have you on board and are committed to helping you make the most of your finances with our expertly curated content. By subscribing to our newsletter, you've taken a step towards smarter spending and a more simplified financial life.</p>
                                        <p>As a subscriber, you’ll receive regular updates featuring:</p>
                                        <ul>
                                            <li>New Budget Plans: Explore our latest and most effective budget plans tailored to various financial goals and lifestyles. Whether you're saving for a big purchase, planning for retirement, or simply looking to better manage your monthly expenses, we've got you covered.</li>
                                            <li>Exclusive Tips and Tricks: Gain insider knowledge on saving money, budgeting, and making informed financial decisions.</li>
                                            <li>Community Highlights: Read success stories and get inspired by other members of the SpendWise community.</li>
                                        </ul>
                                        <p>We are dedicated to providing you with the best content to help you navigate the world of finance with confidence. If you have any questions, suggestions, or topics you'd like us to cover, don't hesitate to reach out.</p>
                                        <p>Welcome aboard, and here's to smarter spending!</p>
                                        <p>Warm regards,</p>
                                        <p>The SpendWise Team</p>
                                        <p>Smart Spending, Simplified</p>
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
            string subject = "Welcome to the SpendWise Community!";
            string urlLogo = "https://i.postimg.cc/HntvP2Pk/logo.png";
            string body = $@"
                                <html>
                                    <body>
                                        <img src='{urlLogo}' alt='Logo' />
                                        <p><strong>Dear Subscriber,</strong></p>
                                        <p>Thank you for joining the SpendWise community! We are thrilled to have you on board and are committed to helping you make the most of your finances with our expertly curated content. By subscribing to our newsletter, you've taken a step towards smarter spending and a more simplified financial life.</p>
                                        <p>As a subscriber, you’ll receive regular updates featuring:</p>
                                        <ul>
                                            <li>New Budget Plans: Explore our latest and most effective budget plans tailored to various financial goals and lifestyles. Whether you're saving for a big purchase, planning for retirement, or simply looking to better manage your monthly expenses, we've got you covered.</li>
                                            <li>Exclusive Tips and Tricks: Gain insider knowledge on saving money, budgeting, and making informed financial decisions.</li>
                                            <li>Community Highlights: Read success stories and get inspired by other members of the SpendWise community.</li>
                                        </ul>
                                        <p>We are dedicated to providing you with the best content to help you navigate the world of finance with confidence. If you have any questions, suggestions, or topics you'd like us to cover, don't hesitate to reach out.</p>
                                        <p>Welcome aboard, and here's to smarter spending!</p>
                                        <p>Warm regards,</p>
                                        <p>The SpendWise Team</p>
                                        <p>Smart Spending, Simplified</p>
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
