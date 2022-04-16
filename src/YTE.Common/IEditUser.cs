using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.Common
{
    public interface IEditUser
    {
        public IFormFile Image { get; set; }
    }
}
