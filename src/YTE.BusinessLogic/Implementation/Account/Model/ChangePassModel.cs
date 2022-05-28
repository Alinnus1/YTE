namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class ChangePassModel : IChangePassword
    {
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
