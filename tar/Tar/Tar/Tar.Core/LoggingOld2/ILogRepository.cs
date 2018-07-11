namespace Tar.Core.LoggingOld2
{
    public interface ILogRepository
    {
        ILogRepository NextRepository { get; set; }
        void WriteToLog(IWriteToLogParameter parameter);
        void Log(IWriteToLogParameter parameter);
    }
}
