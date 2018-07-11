namespace Tar.Core.Serialization.SoapManagement
{
    public interface ISoapMessageConverter
    {
        bool CanConvert<T>() where T : class,new();
        T Convert<T>(string xml) where T : class;
    }
}