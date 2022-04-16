using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class ConfirmationEmailModel
    {
        public bool IsEmailConfirmed { get; set; }
        public string TokenId { get; set; }
        public Guid UserId { get; set; }
        public bool IsTokenExpired { get; set; }

    }
}
