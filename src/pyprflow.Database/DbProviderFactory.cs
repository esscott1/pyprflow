using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprflow.Database
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
            { "dockertest" , "Server=172.17.0.2; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "mssql", "Server=127.0.0.1; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "mssql2017", "Server=127.0.0.1,2250; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
            { "local", "Server=127.0.0.1\\SQLExpress2017; Database=pyprflowlocaldb;User Id = sa; Password=!!nimda1;" },
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
  
}
