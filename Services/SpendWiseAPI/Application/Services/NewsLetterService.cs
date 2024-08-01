using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NewsLetterService
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly IEmailSender _emailSender;
        public NewsLetterService(INewsletterRepository newsletterRepository, IEmailSender emailSender)
        {
            this._newsletterRepository = newsletterRepository;
            _emailSender = emailSender;
        }
        public async Task<bool> AddSubscription(string email)
        {
            var subscriptionCheck = this._newsletterRepository.GetSubscriberByEmail(email);

            if (subscriptionCheck.ToList().Count != 0)
            {
                throw new Exception("You already subscribed this page.");
            }
            var result = await _newsletterRepository.AddSubscription(email);
            if (result == true)
            {
                List<string> emails = new List<string>();
                emails.Add(email);
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
                await _emailSender.SendEmailAsync(emails, subject, body);
            }
            return result;
        }
    }
}
