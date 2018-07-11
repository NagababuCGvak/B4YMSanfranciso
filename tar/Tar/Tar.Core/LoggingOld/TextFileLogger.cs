using System;
using System.IO;

namespace Tar.Core.LoggingOld
{
    public class TextFileLogger : Logger
    {
        private readonly string _filePath;
        private bool _called;

        public TextFileLogger(string filePath)
        {
            _filePath = filePath;
        }
        protected override void SendToLog(string source, LogType logType, string message)
        {
            lock (this)
            {
                using (var writer = new StreamWriter(_filePath, true))
                {
                    if (!_called)
                    {
                        writer.WriteLine();
                        writer.WriteLine();
                        writer.WriteLine(string.Format("{0} > {1} > {2} > {3}", "LogDate".PadRight(20, ' '),
                                                       "LogCategory".PadRight(40, ' '),
                                                       "LogType".PadRight(10, ' '),
                                                       "LogMessage"));
                        _called = true;
                    }
                    writer.WriteLine(string.Format("{0} > {1} > {2} > {3}", DateTime.Now.ToString().PadRight(20, ' '),
                                                   source.PadRight(40, ' '),
                                                   logType.ToString().PadRight(10, ' '),
                                                   message));
                }
            }
        }
    }
}