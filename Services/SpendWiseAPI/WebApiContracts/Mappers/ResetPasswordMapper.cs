using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class ResetPasswordMapper
    {
        public static ResetPassword MapTestToDomain(this PasswordReset model)
        {
            return new ResetPassword
            {
                ID = model.ID,
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword,
                ConfirmPassword = model.ConfirmNewPassword,
            };
        }
    }
}
