using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ReactJokes.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactJokes.Data
{
    public class JokesContextFactory : IDesignTimeDbContextFactory<JokesDbContext>
    {
        public JokesDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}ReactJokes.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new JokesDbContext(config.GetConnectionString("ConStr"));
        }
    }
}