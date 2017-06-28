using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Helpers
{
    public class DbProviderFactory
    {
        private string _dbType;
       
        public IDbProvider Create(string dbType) {

            _dbType = dbType;
            IDbProvider conn = null;
          
            switch (_dbType)
            {
                case ("mssql"):
                    conn = new MSSql();
                    break;
                case ("mssql2017"):
                    conn = new MSSql2017();
                    break;
                case "local":
                    conn = new Local();
                    break;
                case null:
                    conn = new SQLite();
                  //  conn = new MSSql2017();
                    break;
                default:  
                            conn = new SQLite();
                 //   conn = new MSSql2017();
                    break;
            }
            Console.WriteLine("Database Connection string is: {0} ", conn.ConnectionString);
            return conn;
        }

}

    public interface IDbProvider
    {
        string ConnectionString { get; set; }
    }
    public class DbProvider
    {
        public string ConnectionString { get; set; }
       
    }
    class Local : DbProvider, IDbProvider
    {
        public Local()
        {
            ConnectionString = "Server=127.0.0.1,2250;Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;";
        }
    }

    
    class MSSql : DbProvider, IDbProvider
    {
        public MSSql()
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

            Console.WriteLine("MsSQL conn string is: " + sConn);
            ConnectionString = sConn;
           // return "Server=127.0.0.1,2250;Database=pyprflowlocaldb;User Id=sa;Password=!!nimda1;";
        }
    }
    class SQLite : DbProvider, IDbProvider
    {
        public SQLite()
        {
            ConnectionString = "Filename=./Repository.db";
        }

    }
    class MSSql2017 : MSSql, IDbProvider
    {
        public MSSql2017() : base()
        { }
        //public string ConnectionString()
        //{
        //    return "Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;";
        //}

    }
}
