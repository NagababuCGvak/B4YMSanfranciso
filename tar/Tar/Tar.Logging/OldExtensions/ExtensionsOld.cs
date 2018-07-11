using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using Tar.Logging.ObjectContainers;

namespace Tar.Logging.OldExtensions
{
   //static class ExtensionsOld
   //{
   //    #region Object Extensions
   //    public static ILogger LogInfo<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Info<T>(message);
   //    }
   //    public static ILogger LogTrace<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Trace<T>(message);
   //    }
   //    public static ILogger LogDebug<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Debug<T>(message);
   //    }
   //    public static ILogger LogWarn<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Warn<T>(message);
   //    }
   //    public static ILogger LogError<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Error<T>(message);
   //    }
   //    public static ILogger LogFatal<T>(this T source, object message)
   //    {
   //        return LogMan.GetLogger().Fatal<T>(message);
   //    }
   //    #endregion Object Extensions

   //    public static ILogger Trace<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Trace, message);
   //    }
   //    public static ILogger Trace(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Trace, message);
   //    }
   //    public static ILogger Debug<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Debug, message);
   //    }
   //    public static ILogger Debug(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Debug, message);
   //    }

   //    public static ILogger Error<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Error, message);
   //    }
   //    public static ILogger Error(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Error, message);
   //    }

   //    public static ILogger Fatal<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Fatal, message);
   //    }
   //    public static ILogger Fatal(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Fatal, message);
   //    }

   //    public static ILogger Info<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Info, message);
   //    }
   //    public static ILogger Info(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Info, message);
   //    }

   //    public static ILogger Warn<T>(this ILogger logger, object message)
   //    {
   //        return logger.Write(typeof(T), LogLevel.Warn, message);
   //    }
   //    public static ILogger Warn(this ILogger logger, Type sourceType, object message)
   //    {
   //        return logger.Write(sourceType, LogLevel.Warn, message);
   //    }

   //    public static ILogScope CreateScope(this object source, string name)
   //    {
   //        return LogMan.NewScope(name);
   //    }

   //    private static readonly EnvironmentObjectContainer ObjectContainer = new EnvironmentObjectContainer();
   //    private const string IpAddressKey = "Logger.IpAddressKey";

   //    public static void IpAddress(this object source, string ipAddress)
   //    {
   //        ObjectContainer.Set(IpAddressKey, ipAddress);
   //    }

   //    public static string IpAddress(this object source)
   //    {
   //        string hostname = Dns.GetHostName();
   //        if (!string.IsNullOrEmpty(hostname))
   //        {
   //            var ips = Dns.GetHostAddresses(hostname);
   //            var ipFirst = ips.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);

   //            if (ipFirst != null)
   //                return ipFirst.ToString();
   //        }

   //        if(ObjectContainer.Get(IpAddressKey)!=null)
   //        {
   //            return ObjectContainer.Get(IpAddressKey) as string;
   //        }
   //        if (HttpContext.Current != null) return HttpContext.Current.Request.UserHostAddress;
           
   //        var context = OperationContext.Current;
   //        if(context!=null)
   //        {
   //            var prop = context.IncomingMessageProperties;
   //            if(prop!=null)
   //            {
   //                var endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
   //                if(endpoint!=null)
   //                    return endpoint.Address;
   //            }
   //        }
   //        return "";
   //    }
   //}
}
