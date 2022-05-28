using System;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class DetailsUserAccountModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public int Age { get; set; }
        public DateTime JoinDate { get; set; }
        public bool WantsNotifications { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }

    }
}
