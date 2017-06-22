//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;

//namespace pyprflow.Database
//{
//    /// <summary>
//    /// This class with IDbContextFactory needed for command line migrations. but not in EF 1.1.2 :-)
//    /// </summary>
//    class ApiContextFactory : IDbContextFactory<ApiContext>
//    {
//        public ApiContext Create(DbContextFactoryOptions options)
//        {
//            // need to add the connection string factory switch in here
           
//            var builder = new DbContextOptionsBuilder<ApiContext>();
//            //builder.UseSqlServer(
//            //    "Server=127.0.0.1,2250;Database=testcomponentdb;User Id=sa;Password=!!nimda1;",
//            //    b => b.MigrationsAssembly("pyprflow")
//            //    //  "Server=(localdb)\\mssqllocaldb;Database=config;Trusted_Connection=True;MultipleActiveResultSets=true"
//            //    );
//            builder.UseSqlite("Filename=./Repository.db",
//            x => { x.SuppressForeignKeyEnforcement(); x.MigrationsAssembly("pyprflow"); });
//            return new ApiContext(builder.Options);
//        }
//    }
//}