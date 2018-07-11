namespace Tar.Service.Proxy
{
    public interface IServiceProxyFactory<out T>
    {
        string EndPointConfigurationName { get; }
        T CreateChannel();
    }
}