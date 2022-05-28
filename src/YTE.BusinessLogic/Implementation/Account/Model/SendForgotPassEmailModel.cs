namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class SendForgotPassEmailModel
    {
        public bool IsEmailSent { get; set; }
        public bool IsUserFound { get; set; }
        public string Email { get; set; }

    }
}
