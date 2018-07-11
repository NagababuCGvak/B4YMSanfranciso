using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Tar.Core.Mail.Template
{
    [Serializable]
    public class MailTemplate
    {
        public string From { get; set; }

        [XmlIgnore]
        public string Subject { get; set; }
        [XmlElement("Subject", typeof(XmlCDataSection))]
        public XmlCDataSection SubjectCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Subject);
            }
            set
            {
                Subject = value.Value;
            }
        }

        [XmlIgnore]
        public string Body { get; set; }
        [XmlElement("Body", typeof(XmlCDataSection))]
        public XmlCDataSection BodyCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Body);
            }
            set
            {
                Body = value.Value;
            }
        }

        [XmlIgnore]
        public string Signature { get; set; }
        [XmlElement("Signature", typeof(XmlCDataSection))]
        public XmlCDataSection SignatureCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Signature);
            }
            set
            {
                Signature = value.Value;
            }
        }


        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }

        [XmlIgnore]
        public Dictionary<string, object> StaticSources { get; set; }

        public DynamicSourceDefinitionCollection DynamicSourcesDefinition { get; set; }

        public MailTemplate()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            StaticSources = new Dictionary<string, object>();
            DynamicSourcesDefinition = new DynamicSourceDefinitionCollection();
        }
    }
}