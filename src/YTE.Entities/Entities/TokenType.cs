using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.Entities
{
    public partial class TokenType
    {
        public TokenType()
        {
            Tokens = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
    }
}
