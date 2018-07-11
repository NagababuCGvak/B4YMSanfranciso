using System;
using Tar.Core.Extensions;

namespace Tar.Core.LoggingOld
{
    public static class LoggerExtensionMethods
    {
        public static void Log<T>(this ILogger logger, LogType logType, string message, params object[] parameters)
        {
            logger.Log(string.Format("{0}, {1}", typeof (T).FullName, typeof (T).Assembly.GetName().Name),
                       logType, message, parameters);
        }

        public static void Info<T>(this ILogger logger, string message, params object[] parameters)
        {
            logger.Log<T>(LogType.Info, message, parameters);
        }

        public static void Error<T>(this ILogger logger, string message, params object[] parameters)
        {
            logger.Log<T>(LogType.Error, message, parameters);
        }

        public static void Warning<T>(this ILogger logger, string message, params object[] parameters)
        {
            logger.Log<T>(LogType.Warning, message, parameters);
        }

        public static void Trace<T>(this ILogger logger, string message, params object[] parameters)
        {
            logger.Log<T>(LogType.Trace, message, parameters);
        }

        public static void Exception<T>(this ILogger logger, Exception exception)
        {
            logger.Error<T>(exception.ToJson());
        }
    }
}