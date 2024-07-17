using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NewsletterRepository:INewsletterRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public NewsletterRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<Newsletter> GetSubscriberByEmail(string email)
        {
            var query = "SELECT [subscription_id],[email] from [SpendWise].[Subscriptions] WHERE [email]=@Email";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<Newsletter>(query, new { Email = email}).ToList();
            return file;
        }
        public async Task<bool> AddSubscription(string email)
        {
            var query = "INSERT INTO [SpendWise].[Subscriptions] ([subscription_id],[email]) VALUES (NEWID(), @Email)";
            
            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, new { Email = email });
            return result != 0;
        }
    }
}
