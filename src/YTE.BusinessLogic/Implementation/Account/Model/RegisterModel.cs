using System;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public DateTime Age { get; set; }
        public int? GenderId { get; set; }


    }
}
