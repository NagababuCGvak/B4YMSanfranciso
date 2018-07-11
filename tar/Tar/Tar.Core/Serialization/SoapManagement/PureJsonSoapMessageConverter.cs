namespace Tar.Core.Serialization.SoapManagement
{
    public class PureJsonSoapMessageConverter : ISoapMessageConverter
    {
        public bool CanConvert<T>() where T : class, new()
        {
            return typeof(T) == typeof(PureJson);
        }

        public T Convert<T>(string xml) where T : class
        {
            return (T) ((object) new PureJson {Json = SerializationHelper.ToJsonFromXml(xml)});
        }
    }
}