using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Helpers
{
    class ConnectionStringFactory
    {
        static public IConnectionString GetConnetionString() {

            IConnectionString conn = null;
            switch (Environment.GetEnvironmentVariable("DatabaseType").ToLower())
            {
                case ("mssql"):
                    conn = new MSSql();
                    break;
                case ("mssql2017"):
                    conn = new MSSql2017();
                    break;
                default:
                    conn = new SQLite();
                    break;
            }
            return conn;
        }

}

    interface IConnectionString
    {
        string ConnectionString();
    }
    class MSSql : IConnectionString
    {
        public string ConnectionString()
        {
            return "Server=EricLaptop\\DEV2014;Database=pyprflowlocaldb;User Id=sa;Password=!!nimda;";
        }
    }
    class SQLite : IConnectionString
    {
        public string ConnectionString()
        {
            return "Filename=./Repository.db";
        }

    }
    class MSSql2017 : IConnectionString
    {
        public string ConnectionString()
        {
            return "Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;";
        }

    }
}
