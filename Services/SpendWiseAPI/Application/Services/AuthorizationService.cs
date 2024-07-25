using Application.Interfaces;
using Domain;
using System.Net;

namespace Application.Services
{
    public class AuthorizationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IIdentityHandler _identityHandler;

        public AuthorizationService(IPasswordHasher passwordHasher,
            IAuthenticationRepository authenticationRepository,
            IIdentityHandler identityHandler)
        {
            _passwordHasher = passwordHasher;
            _identityHandler = identityHandler;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<bool> RegisterUser(UserCredentials credentials)
        {
            var userCheck = await this._authenticationRepository.GetUser(credentials.Email);

            if (userCheck.ToList().Count != 0)
            {
                throw new Exception("User already registered");
                //throw new NullReferenceException("User already registered");
            }
            if (credentials.Password != credentials.ConfirmPassword)
            {
                throw new Exception("Passwords are different");
            }

            var hashedPassword = this._passwordHasher.Hash(credentials.Password);
            var registerResult = await this._authenticationRepository.RegisterUser(new UserCredentials
            {
                Name = credentials.Name,
                Email = credentials.Email,
                Password = hashedPassword,
                Phone = credentials.Phone,
                Role = "user"
            });

            return registerResult;
        }

        public async Task<User> LoginUser(UserCredentials credentials)
        {
            var userHashed = await this._authenticationRepository.GetUser(credentials.Email);

            if (userHashed.ToList().Count==0 || !_passwordHasher.Verify(userHashed.FirstOrDefault().Password, credentials.Password))
            {
                throw new Exception("Username or password are incorrect");
            }

            var result = new User
            {
                ID = userHashed.FirstOrDefault().user_id,
                Name = userHashed.FirstOrDefault().Name, 
                Role = userHashed.FirstOrDefault().Role,
            };

            var jwtToken = this._identityHandler.GenerateToken(result);
            result.JwtToken = jwtToken;

            return result;
        }

        public async Task<bool> GiveUserAdminRights(string email)
        {
            var userCheck = await this._authenticationRepository.GetUser(email);

            if (userCheck.ToList().Count == 0)
            {
                throw new Exception("User is not registered");
            }

            var result = await this._authenticationRepository.GiveUserAdminRights(email);

            return result;
        }

        public async Task<bool> SaveAccountSettings(User credentials)
        {
            var userCheck = await this._authenticationRepository.GetUserByID(credentials.ID);

            if (userCheck.ToList().Count == 0)
            {
                throw new Exception("User are not registered");
            }

            var registerResult = await this._authenticationRepository.SaveAccountSettings(new UserCredentials
            {
                user_id = credentials.ID,
                Name = credentials.Name,
                Email = credentials.Email,
                Password = userCheck.FirstOrDefault().Password,
                Phone = credentials.Phone,
                Role = userCheck.FirstOrDefault().Role
            });

            return registerResult;
        }
        public async Task<bool> ResetPassword(ResetPassword resetPassword)
        {
            var userCheck = await this._authenticationRepository.GetUserByID(resetPassword.ID);

            if (userCheck.ToList().Count == 0)
            {
                throw new Exception("User are not registered");
            }

            if (!_passwordHasher.Verify(userCheck.FirstOrDefault().Password, resetPassword.CurrentPassword))
            {
                throw new Exception("Current password is incorrect!");
            }

            if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
            {
                throw new Exception("Passwords are different");
            }

            var hashedNewPassword = this._passwordHasher.Hash(resetPassword.NewPassword);
            var registerResult = await this._authenticationRepository.ResetPassword(new UserCredentials
            {
                user_id = resetPassword.ID,
                Name = userCheck.FirstOrDefault().Name,
                Email = userCheck.FirstOrDefault().Email,
                Password = hashedNewPassword,
                Phone = userCheck.FirstOrDefault().Phone,
                Role = userCheck.FirstOrDefault().Role,
            });

            return registerResult;
        }
    }
}
