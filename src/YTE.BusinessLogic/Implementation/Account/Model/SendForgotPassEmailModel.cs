﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class SendForgotPassEmailModel
    {
        public bool IsEmailSent { get; set; }
        public bool IsUserFound { get; set; }
        public string Email { get; set; }

    }
}