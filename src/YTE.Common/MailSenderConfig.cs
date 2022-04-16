using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.Common
{
    public class MailSenderConfig
    {
        
        public string Displayname { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultsCredentials { get; set; }
    }
}
