using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class DeleteUserAccountModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
