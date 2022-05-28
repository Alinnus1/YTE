namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public interface IChangePassword
    {
        public string ConfirmNewPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
