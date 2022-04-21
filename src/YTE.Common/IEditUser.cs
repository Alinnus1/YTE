using Microsoft.AspNetCore.Http;

namespace YTE.Common
{
    public interface IEditUser
    {
        public IFormFile Image { get; set; }
    }
}
