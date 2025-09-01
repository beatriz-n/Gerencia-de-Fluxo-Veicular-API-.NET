
using FluxoVeicular.App.Client.Response;
using FluxoVeicular.ServiceDefaults.Context;
using Microsoft.EntityFrameworkCore;

namespace FluxoVeicular.ServiceDefaults.Services
{
    public class VeiculoPlacaService
    {

        private readonly FluxoVeicularContext _context;
        public VeiculoPlacaService(FluxoVeicularContext context)
        {
            _context = context;
        }

        public async Task<VeiculoPlacaResponse> GetVeiculoByPlacaAsync(string placa)
        {
            var veiculo = await _context.Veiculos
                .AsNoTracking() // melhora performance em consulta
                .FirstOrDefaultAsync(v => v.Placa == placa);

                return new VeiculoPlacaResponse
                {
                    Placa = veiculo?.Placa
                };

        }

    }
}
