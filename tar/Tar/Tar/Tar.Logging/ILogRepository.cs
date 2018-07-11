namespace Tar.Logging
{
    public interface ILogRepository
    {
        ILogRepository NextRepository { get; set; }
        void WriteToLog(IWriteToLogParameter parameter);
        void DoLog(IWriteToLogParameter parameter);
    }
}
