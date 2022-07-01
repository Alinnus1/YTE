using Microsoft.AspNetCore.Http;
using System;
using YTE.Common;

namespace YTE.BusinessLogic.Implementation.Account.Model
{
    public class EditUserAccountModel : IEditUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Age { get; set; }
        public int? GenderId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool WantsNotifications { get; set; }
        public IFormFile Image { get; set; }
    }
}
