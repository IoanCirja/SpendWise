using Dapper;
using System.Data;

using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class AuthenticationRepository: IAuthenticationRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public AuthenticationRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<IEnumerable<UserCredentials>> GetUser(string email)
        {
            var sql = "SELECT [user_id], [name], [email], [password], [phone], [role] FROM [SpendWise].[Users] WHERE [email] = @Email";
            var connection = _databaseContext.GetDbConnection();
            var users = await connection.QueryAsync<UserCredentials>(sql, new {Email = email});
            return users;
        }
        public async Task<IEnumerable<UserCredentials>> GetUserByID(Guid user_id)
        {
            var sql = "SELECT [user_id], [name], [email], [password], [phone], [role] FROM [SpendWise].[Users] WHERE [user_id] = @UserID";
            var connection = _databaseContext.GetDbConnection();
            var users = await connection.QueryAsync<UserCredentials>(sql, new { UserID = user_id });
            return users;
        }


        public async Task<bool> RegisterUser(UserCredentials credentials)
        {
            var query = "INSERT INTO [SpendWise].[Users] ([user_id], [name], [password], [email], [phone], [role]) VALUES (NEWID(), @Name, @Password, @Email, @Phone, @Role)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", credentials.Name, DbType.String);
            parameters.Add("Password", credentials.Password, DbType.String);
            parameters.Add("Email", credentials.Email, DbType.String);
            parameters.Add("Phone", credentials.Phone, DbType.String);
            parameters.Add("Role", credentials.Role, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> GiveUserAdminRights(string email)
        {
            var sql = "UPDATE [SpendWise].[Users] SET [role] = 'admin' WHERE [email] = @Email";
            
            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, new { Email = email });
            return result != 0;
        }
        public async Task<bool> SaveAccountSettings(UserCredentials credentials)
        {
            var query = "UPDATE [SpendWise].[Users] SET [name] = @Name, [email] = @Email, [phone] = @Phone WHERE [user_id] = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId",credentials.user_id, DbType.Guid);
            parameters.Add("Name", credentials.Name, DbType.String);
            parameters.Add("Email", credentials.Email, DbType.String);
            parameters.Add("Phone", credentials.Phone, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
        public async Task<bool> ResetPassword(UserCredentials credentials)
        {
            var query = "UPDATE [SpendWise].[Users] SET [password] = @Password WHERE [user_id] = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", credentials.user_id, DbType.Guid);
            parameters.Add("Password", credentials.Password, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
        public List<string> GetAdminEmail()
        {
            var sql = "SELECT [email] FROM [SpendWise].[Users] WHERE [role] = 'admin'";
            var connection = _databaseContext.GetDbConnection();
            var users = connection.Query<string>(sql).ToList();
            return users;
        }



        public async Task<List<string>> GetAllUsersEmails()
        {
            var query = "SELECT [Email] From [SpendWise].[Users] WHERE [Role] = 'user'";

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.QueryAsync<string>(query);
            return result.ToList();
        }
    }
}
