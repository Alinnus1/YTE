using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.Common;

namespace YTE.Entities
{
    public partial class ForbiddenWord : IEntity
    {
        public int Id { get; set; }
        public string Word { get; set; }
    }
}
