using System;
using System.Threading.Tasks;

namespace DependencyInjection.Services
{
    public interface ILogService
    {
        string GetLogName();
    }

    public class LogService:ILogService, IDisposable,IAsyncDisposable
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


        public async ValueTask DisposeAsync()
        {
            //Release Resources Async
        }

        public void Dispose()
        {
            //Release Resources
        }
    }
}