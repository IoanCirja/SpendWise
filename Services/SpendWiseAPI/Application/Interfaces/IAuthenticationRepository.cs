using Domain;

namespace Application.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<IEnumerable<UserCredentials>> GetUser(string email);
        Task<IEnumerable<UserCredentials>> GetUserByID(Guid user_id);
        Task<bool> RegisterUser(UserCredentials credentials);
        Task<bool> GiveUserAdminRights(string email);
        Task<bool> SaveAccountSettings(UserCredentials credentials);
        Task<bool> ResetPassword(UserCredentials credentials);
        List<string> GetAdminEmail();
    }
}
