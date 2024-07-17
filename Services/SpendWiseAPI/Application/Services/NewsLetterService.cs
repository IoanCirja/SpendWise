using Application.Interfaces;
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
        private INewsletterRepository _newsletterRepository;
        public NewsLetterService(INewsletterRepository newsletterRepository)
        {
            this._newsletterRepository = newsletterRepository;
        }
        public async Task<bool> AddSubscription(string email)
        {
            var subscriptionCheck = this._newsletterRepository.GetSubscriberByEmail(email);

            if (subscriptionCheck.ToList().Count != 0)
            {
                throw new Exception("You already subscribed this page.");
            }
            return await _newsletterRepository.AddSubscription(email);
        }
    }
}
