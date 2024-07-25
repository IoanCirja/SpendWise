using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class ContactUsMapper
    {
        public static ContactUs MapTestToDomain(this ContactUsContract contactUsContract)
        {
            return new ContactUs
            {
                firstName = contactUsContract.firstName,
                lastName = contactUsContract.lastName,
                email = contactUsContract.email,
                message = contactUsContract.message,
                status = "RECEIVED",
            };
        }
    }
}
