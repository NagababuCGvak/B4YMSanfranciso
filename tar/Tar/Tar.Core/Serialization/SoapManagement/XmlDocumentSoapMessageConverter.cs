using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Tar.Core.Serialization.SoapManagement
{
    public class XmlDocumentSoapMessageConverter : ISoapMessageConverter
    {
        public bool CanConvert<T>() where T : class, new()
        {
            return typeof(T) == typeof(XDocument) || typeof(T) == typeof(XmlDocument);
        }

        public T Convert<T>(string xml) where T : class
        {
            if (typeof(XDocument) == typeof(T))
            {
                var xDocument = XDocument.Load(new StringReader(xml));
                return (T)((object)xDocument);
            }
            if (typeof(XmlDocument) == typeof(T))
            {
                
                var xmlDocument=new XmlDocument();
                xmlDocument.Load(new StringReader(xml));
                return (T)((object)xmlDocument);
            }
            return null;
        }
    }
}