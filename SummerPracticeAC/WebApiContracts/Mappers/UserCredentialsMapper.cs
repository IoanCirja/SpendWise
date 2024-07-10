using Domain;

namespace WebApiContracts.Mappers
{
    public static class UserCredentialsMapper
    {

        public static UserCredentials MapTestToDomain(this UserCredentialsContract credentials)
        {
            return new UserCredentials
            {
                Name = credentials.Name,
                Email = credentials.Email,
                Password = credentials.Password,
                ConfirmPassword = credentials.ConfirmPassword,
                Phone = credentials.Phone,
            };
        } 

    }
}
