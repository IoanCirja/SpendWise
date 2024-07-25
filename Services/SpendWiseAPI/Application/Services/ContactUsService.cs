using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ContactUsService
    {
        private IContactUsRepository _contactUsRepository;
        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository; 
        }
        public async Task<bool> AddFormContactUs(ContactUs contactUs)
        {
            return await _contactUsRepository.AddFormContactUs(contactUs);
        }

    }
}
