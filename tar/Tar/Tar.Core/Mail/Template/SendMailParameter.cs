using System.Collections.Generic;

namespace Tar.Core.Mail.Template
{
    public class SendMailParameter
    {
        public string Key { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool UseDynamicSource { get; set; }
        public List<string> To { get; private set; }
        public List<string> Cc { get; private set; }
        public List<string> Bcc { get; private set; }

        public SendMailParameter(string key)
        {
            Key = key;
            Parameters = new Dictionary<string, object>();
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }
    }
}