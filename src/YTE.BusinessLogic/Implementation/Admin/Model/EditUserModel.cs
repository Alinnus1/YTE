using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using YTE.Common;

namespace YTE.BusinessLogic.Implementation.Admin.Model
{
    public class EditUserModel : IEditUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public DateTime Age { get; set; }
        public int? GenderId { get; set; }
        public string NewPassword { get; set; }
        public IFormFile Image { get; set; }

        public List<int> selectedRoles { get; set; }
    }
}
