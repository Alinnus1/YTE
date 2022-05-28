using System;

namespace YTE.BusinessLogic.Implementation.Admin.Model
{
    public class ListUserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public DateTime JoinDate { get; set; }

    }
}
