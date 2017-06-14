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
          
            switch (Environment.GetEnvironmentVariable("pfdatabasetype"))
            {
                case ("mssql"):
                    conn = new MSSql();
                    break;
                case ("mssql2017"):
                    conn = new MSSql2017();
                    break;
                case null:
                    conn = new SQLite();
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
            //string dbconnstr = Environment.GetEnvironmentVariable("pfdatabasetype");
            //if (String.IsNullOrEmpty(dbconnstr))
            //    return dbconnstr;

            var dbtype = Environment.GetEnvironmentVariable("pfdatabasetype");
            var dbhost = Environment.GetEnvironmentVariable("pfdbhost");
            var dbinstance = Environment.GetEnvironmentVariable("pfmsdbinstance");
            var dbname = Environment.GetEnvironmentVariable("pfdbname");
            var dbport = Environment.GetEnvironmentVariable("pfdbport");
            var dbid = Environment.GetEnvironmentVariable("pfdbid"); 
            var dbpw = Environment.GetEnvironmentVariable("pfdbpw");
            string sConn;
            if(String.IsNullOrEmpty(dbport))
                sConn = string.Format("Server={0}\\{1};Database={2};User Id={3};Password={4};",
                    dbhost, dbinstance, dbname, dbid, dbpw, dbport);
            else
                sConn = string.Format("Server={0},{5};Database={2};User Id={3};Password={4};",
                    dbhost, dbinstance, dbname, dbid, dbpw, dbport);

             return sConn;

           // return "Server=127.0.0.1,2250;Database=pyprflowlocaldb;User Id=sa;Password=!!nimda1;";
        }
    }
    class SQLite : IConnectionString
    {
        public string ConnectionString()
        {
            return "Filename=./Repository.db";
        }

    }
    class MSSql2017 : MSSql, IConnectionString
    {
        //public string ConnectionString()
        //{
        //    return "Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;";
        //}

    }
}
