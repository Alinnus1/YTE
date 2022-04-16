using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class ForgotPassModel : IChangePassword
    {
        public string TokenId { get; set; }
        public Guid UserId { get; set; }
        public bool AreCredentialsValid { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
