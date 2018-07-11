using Newtonsoft.Json.Linq;

namespace Tar.Core.Serialization.SoapManagement
{
    public class JObjectSoapMessageConverter : ISoapMessageConverter
    {
        public bool CanConvert<T>() where T : class, new()
        {
            return typeof (T) == typeof (JObject);
        }

        public T Convert<T>(string xml) where T : class
        {
            return (T) ((object) SerializationHelper.ToJObjectFromXml(xml));
        }
    }
}