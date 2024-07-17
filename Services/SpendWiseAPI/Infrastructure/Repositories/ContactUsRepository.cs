using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ContactUsRepository:IContactUsRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public ContactUsRepository(IDatabaseContext databaseContext) 
        {
            this._databaseContext = databaseContext;
        }
        public async Task<bool> AddFormContactUs(ContactUs contactUs)
        {
            var query = "INSERT INTO [SpendWise].[Contact] ([contact_id],[firstName],[lastName],[email],[message],[status]) VALUES (NEWID(), @FirstName, @LastName, @Email, @Message, @Status)";

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", contactUs.firstName, DbType.String);
            parameters.Add("LastName", contactUs.lastName, DbType.String);
            parameters.Add("Email", contactUs.email, DbType.String);
            parameters.Add("Message", contactUs.message, DbType.String);
            parameters.Add("Status", contactUs.status, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
    }
}
