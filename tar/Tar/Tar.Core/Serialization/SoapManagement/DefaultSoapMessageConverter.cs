using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Tar.Core.Serialization.SoapManagement
{
    public class DefaultSoapMessageConverter : ISoapMessageConverter
    {
        public bool CanConvert<T>() where T : class,new()
        {
            return true;
        }

        public T Convert<T>(string xml) where T : class
        {
            var dataContractSerializer = new DataContractSerializer(typeof (T));

            var result = dataContractSerializer
                .ReadObject(new XmlTextReader(new StringReader(xml)));
            return (T) result;
        }
    }
}