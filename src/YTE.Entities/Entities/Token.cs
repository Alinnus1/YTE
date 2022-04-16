using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.Common;

namespace YTE.Entities
{
    public partial class Token : IEntity
    {
        public string Id { get; set; }
        public int TypeId { get; set; }
        public DateTime Date { get; set; }

        public virtual TokenType Type { get; set; }
    }
}
