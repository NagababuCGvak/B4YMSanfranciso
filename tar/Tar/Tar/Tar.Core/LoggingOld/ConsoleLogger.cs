using System;

namespace Tar.Core.LoggingOld
{
    public class ConsoleLogger : Logger
    {
        private bool _called;
        protected override void SendToLog(string source, LogType logType, string message)
        {
            if (!_called)
            {
                Console.WriteLine("{0} > {1} > {2} > {3}", "LogDate".PadRight(20, ' '), "LogCategory".PadRight(40, ' '),
                                  "LogType".PadRight(10, ' '),
                                  "LogMessage");
                _called = true;
            }
            Console.WriteLine("{0} > {1} > {2} > {3}", DateTime.Now.ToString().PadRight(20, ' '),
                              source.PadRight(40, ' '),
                              logType.ToString().PadRight(10, ' '),
                              message);
        }
    }
}