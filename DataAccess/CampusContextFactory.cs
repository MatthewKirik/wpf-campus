using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CampusContextFactory : IDesignTimeDbContextFactory<CampusContext>
    {
        public CampusContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<CampusContext>();
            //optionsBuilder.UseSqlite($"Data Source=./campus.db");
            return new CampusContext(/*optionsBuilder.Options*/);
        }
    }
}
