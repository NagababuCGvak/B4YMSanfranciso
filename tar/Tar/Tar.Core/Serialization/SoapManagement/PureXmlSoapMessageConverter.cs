namespace Tar.Core.Serialization.SoapManagement
{
    public class PureXmlSoapMessageConverter : ISoapMessageConverter
    {
        public bool CanConvert<T>() where T : class, new()
        {
            return typeof(T) == typeof(PureXml);
        }

        public T Convert<T>(string xml) where T : class
        {
            return (T) ((object) new PureXml {Xml = xml});
        }
    }
}