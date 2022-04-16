using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public interface IChangePassword
    {
        public string ConfirmNewPassword { get; set; }
        public string NewPassword{ get; set; }
    }
}
