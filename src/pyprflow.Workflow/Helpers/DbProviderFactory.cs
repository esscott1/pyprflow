using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Workflow.Helpers
{
    public enum DbProviderType
    {
        sqlite = 0,
        mssql = 1,
        dockertest = 2

    }
    public class DbProviderFactory
    {
        internal static Dictionary<string, Action<DbContextOptionsBuilder>> _DbProviderStrategy =
          new Dictionary<string, Action<DbContextOptionsBuilder>>() {
             { "dockertest",  o => o.UseSqlServer(conn, m => m.MigrationsAssembly("pyprflow")) },
             { "mssql2017", o => o.UseSqlServer(conn, m => m.MigrationsAssembly("pyprflow")) },
             { "local", o => o.UseSqlServer(conn, m => m.MigrationsAssembly("pyprflow")) },
             { "mssql", o => o.UseSqlServer(conn, m => m.MigrationsAssembly("pyprflow")) },
             {"sqlite", o => o.UseSqlite(conn, m => { m.SuppressForeignKeyEnforcement(); m.MigrationsAssembly("pyprflow"); }) }

           };
        internal static Dictionary<string, string> _conn = new Dictionary<string, string>
        {
            { "dockertest" , "Server=10.0.0.25; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "mssql", "Server=127.0.0.1; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "mssql2017", "Server=127.0.0.1,2250; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "local", "Server=127.0.0.1,2250; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "sqlite" , "Filename=./Repository.db" }
        };
        internal static string conn = string.Empty;
      
        public IDbProvider Create(string dbType = "local") {

            DbProvider concreteProvder = new DbProvider();
            conn = _conn[dbType];
            concreteProvder.dbContext = _DbProviderStrategy[dbType];
            concreteProvder.ConnectionString = conn;
            return concreteProvder;
          
        }

}

    public interface IDbProvider
    {
        string ConnectionString { get; set; }
        Action<DbContextOptionsBuilder> dbContext { get; set; }
    }
    public class DbProvider: IDbProvider
    {
        public string ConnectionString { get; set; }
        public Action<DbContextOptionsBuilder> dbContext { get; set; }
 

    }
    //class Local : DbProvider, IDbProvider
    //{
    //    public Local()
    //    {
    //        ConnectionString = "Server=127.0.0.1,2250;Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;";
    //    }
    //}

    
    //class MSSql : DbProvider, IDbProvider
    //{
    //    public MSSql()
    //    {
    //        //string dbconnstr = Environment.GetEnvironmentVariable("pfdatabasetype");
    //        //if (String.IsNullOrEmpty(dbconnstr))
    //        //    return dbconnstr;

    //        var dbtype = Environment.GetEnvironmentVariable("pfdatabasetype");
    //        var dbhost = Environment.GetEnvironmentVariable("pfdbhost");
    //        var dbinstance = Environment.GetEnvironmentVariable("pfmsdbinstance");
    //        var dbname = Environment.GetEnvironmentVariable("pfdbname");
    //        var dbport = Environment.GetEnvironmentVariable("pfdbport");
    //        var dbid = Environment.GetEnvironmentVariable("pfdbid"); 
    //        var dbpw = Environment.GetEnvironmentVariable("pfdbpw");
    //        string sConn;
    //        if (String.IsNullOrEmpty(dbport))
    //        {
    //            if(String.IsNullOrEmpty(dbinstance))
    //            sConn = string.Format("Server={0};Database={2};User Id={3};Password={4};",
    //                dbhost, dbinstance, dbname, dbid, dbpw, dbport);
    //            else
    //                sConn = string.Format("Server={0}\\{1};Database={2};User Id={3};Password={4};",
    //                dbhost, dbinstance, dbname, dbid, dbpw, dbport);

    //        }
    //        else
    //            sConn = string.Format("Server={0},{5};Database={2};User Id={3};Password={4};",
    //                dbhost, dbinstance, dbname, dbid, dbpw, dbport);

    //        Console.WriteLine("MsSQL conn string is: " + sConn);
    //        ConnectionString = sConn;
    //       // return "Server=127.0.0.1,2250;Database=pyprflowlocaldb;User Id=sa;Password=!!nimda1;";
    //    }
    //}
    //class SQLite : DbProvider, IDbProvider
    //{
    //    public SQLite()
    //    {
    //        ConnectionString = "Filename=./Repository.db";
    //    }

    //}
    //class MSSql2017 : MSSql, IDbProvider
    //{
    //    public MSSql2017() : base()
    //    { }
    //    //public string ConnectionString()
    //    //{
    //    //    return "Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;";
    //    //}

    //}
}
