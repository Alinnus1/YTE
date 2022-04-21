using System;
using System.Collections.Generic;

namespace YTE.Common.DTOS
{
    public class CurrentUserDto
    {
        public CurrentUserDto()
        {
            Roles = new List<string>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}
