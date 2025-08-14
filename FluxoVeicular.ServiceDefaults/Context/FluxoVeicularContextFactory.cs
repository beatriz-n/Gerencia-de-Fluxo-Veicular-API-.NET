using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FluxoVeicular.ServiceDefaults.Context
{
    internal class FluxoVeicularContextFactory : IDesignTimeDbContextFactory<FluxoVeicularContext>
    {
        public FluxoVeicularContext CreateDbContext(string[] args)
        {
            // Caminho para o appsettings.json do projeto de startup
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FluxoVeicularContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Usando PostgreSQL
            optionsBuilder.UseNpgsql(connectionString);

            return new FluxoVeicularContext(optionsBuilder.Options);
        }
    }
}