using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContracts;

namespace WebApiContracts.Mappers
{
    public static class UserAccountMapper
    {
        public static User MapTestToDomain(this UserAccount userAccount)
        {
            return new User
            {
                ID = Guid.Parse(userAccount.ID),
                Name = userAccount.Name,
                Email = userAccount.Email,
                Phone = userAccount.Phone,
            };
        }
    }
}
