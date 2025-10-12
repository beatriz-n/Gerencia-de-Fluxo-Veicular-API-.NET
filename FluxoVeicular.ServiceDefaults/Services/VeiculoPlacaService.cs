using FluxoVeicular.App.Client.Enum;
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

        public async Task<VeiculoPlacaResponse?> GetVeiculoByPlacaAsync(string placa)
        {
            var veiculo = await _context.Veiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Placa == placa);
            var acesso = false;
            if (veiculo is null)
            {
                acesso = false;
            }
            else
            {
                acesso = true;
            }
               

            return new VeiculoPlacaResponse
            {
                Placa = veiculo?.Placa,
                Acesso = acesso
            };
        }

        /// <summary>
        /// Decide o próximo tipo de acesso do veículo.
        /// </summary>
        public async Task<TipoAcesso> GetProximoAcessoAsync(string placa)
        {
            var existeVeiculo = await _context.Veiculos
                .AsNoTracking()
                .AnyAsync(v => v.Placa == placa);

            var ultimoLog = await _context.Logs
                .AsNoTracking()
                .Where(l => l.Placa == placa)
                .OrderByDescending(l => l.DataHora)
                .FirstOrDefaultAsync();

            if (ultimoLog is null || ultimoLog.Tipo == TipoAcesso.Saida.ToString())
                return TipoAcesso.Entrada;

            return TipoAcesso.Saida;
        }
    }
}
