namespace DependencyInjection.Services
{
    public interface ILogService
    {
        string GetLogName();
    }

    public class LogService:ILogService
    {
        string _logName;

        public LogService(string logName)
        {
            _logName = logName;
        }

        public string GetLogName()
        {
            return _logName;
        }


        
    }
}