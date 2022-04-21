using System;
using System.Collections.Generic;

namespace YTE.BusinessLogic.Implementation.Admin.Model
{
    public class DetailsUserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public int Age { get; set; }
        public DateTime JoinDate { get; set; }
        public string Gender { get; set; }
        public List<string> Roles { get; set; }
        public string Image { get; set; }
    }
}
