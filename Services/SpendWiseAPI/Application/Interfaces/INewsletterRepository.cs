using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INewsletterRepository
    {
        List<Newsletter> GetSubscriberByEmail(string email);
        Task<bool> AddSubscription(string email);
    }
}
