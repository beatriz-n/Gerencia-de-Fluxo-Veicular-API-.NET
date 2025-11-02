using FluxoVeicular.ServiceDefaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace FluxoVeicular.ServiceDefaults.Context
{
    public class FluxoVeicularContext : DbContext
    {
        public FluxoVeicularContext(DbContextOptions<FluxoVeicularContext> options)
            : base(options) { }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
