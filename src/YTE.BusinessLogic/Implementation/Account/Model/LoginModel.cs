namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
        public bool AreCredentialsInvalid { get; set; }
        public bool EmailUnConfirmed { get; set; }

    }
}
