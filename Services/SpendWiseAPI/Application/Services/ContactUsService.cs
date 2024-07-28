using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application.Services
{
    public class ContactUsService
    {
        private IContactUsRepository _contactUsRepository;
        private IAuthenticationRepository _authenticationRepository;
        private IEmailSender _emailSender;
        public ContactUsService(IContactUsRepository contactUsRepository, IAuthenticationRepository authenticationRepository, IEmailSender emailSender)
        {
            _contactUsRepository = contactUsRepository;
            _authenticationRepository = authenticationRepository;
            _emailSender = emailSender;
        }
        public async Task<bool> AddFormContactUs(ContactUs contactUs)
        {
            var result = await _contactUsRepository.AddFormContactUs(contactUs);
            if (result == true)
            {
                var emails = _authenticationRepository.GetAdminEmail();
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
                await _emailSender.SendEmailAsync(emails, subject, body);
            }
            return result;
        }

    }
}
