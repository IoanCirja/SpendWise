using Domain;

namespace WebApiContracts.Mappers
{
    public static class UserCredentialsMapper1
    {

        public static UserCredentials MapTestToDomain(this UserCredentialsContract1 credentials)
        {
            return new UserCredentials
            {
                Email = credentials.Email,
                Password = credentials.Password,
            };
        }

    }
}
