using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace pyprflow.Database
{
    class ApiContextFactory : IDbContextFactory<ApiContext>
    {
        public ApiContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<ApiContext>();
            builder.UseSqlServer(
                "Server=127.0.0.2,2250;Database=testcomponentdb;User Id=sa;Password=!!nimda1;"
              //  "Server=(localdb)\\mssqllocaldb;Database=config;Trusted_Connection=True;MultipleActiveResultSets=true"
                );

            return new ApiContext(builder.Options);
        }
    }
}