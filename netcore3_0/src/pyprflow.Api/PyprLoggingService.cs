using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Api
{
    public interface IPyprLoggingService
    {
        public void WriteLog(string message);
    }
    public class PyprLoggingService: IPyprLoggingService
    {
        public ILogger Logger { get; set; }
        public void WriteLog(string message)
        {
            Logger.LogInformation(message);
        }
    }
}
