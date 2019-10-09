using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Api
{
    public class DatabaseSettings
    {
        public string DatabaseType { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string Port { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
    }
}
