using FluxoVeicular.App.Client.Response.Dashboards;
using FluxoVeicular.ServiceDefaults.Context;
using Microsoft.EntityFrameworkCore;

namespace FluxoVeicular.Infra.Services
{
    public class DashboardService
    {
        private readonly FluxoVeicularContext _context;

        public DashboardService(FluxoVeicularContext context)
        {
            _context = context;
        }

        DateTime hoje = DateTime.UtcNow.Date;

        public async Task<DashboardsTotaisResponse> GetDashboardsTotalHojeAsync()
        {

    
            // Entradas e saídas de HOJE
            var totalEntradas = await _context.Logs
                .AsNoTracking()
                .Where(l => l.Tipo == "Entrada" && l.DataHora.Date == hoje)
                .CountAsync();
        
            var totalSaidas = await _context.Logs
                .AsNoTracking()
                .Where(l => l.Tipo == "Saida" && l.DataHora.Date == hoje)
                .CountAsync();

            // Total REAL na garagem (todos os tempos)
            var totalVeiculosGaragem = await _context.Logs
                .AsNoTracking()
                .Where(l => l.Tipo == "Entrada")
                .CountAsync() - 
                await _context.Logs
                .AsNoTracking()
                .Where(l => l.Tipo == "Saida")
                .CountAsync();

            return new DashboardsTotaisResponse
            {
                TotalEntradasHoje = totalEntradas,
                TotalSaidasHoje = totalSaidas,
                TotalVeiculosGaragem = totalVeiculosGaragem // Ou use (totalEntradas - totalSaidas) para mostrar só de hoje
            };
        }
    }
}
