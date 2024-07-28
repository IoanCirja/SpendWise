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
                await _emailSender.SendEmailAsync(emails, subject, body);
            }
            return result;
        }
    }
}
