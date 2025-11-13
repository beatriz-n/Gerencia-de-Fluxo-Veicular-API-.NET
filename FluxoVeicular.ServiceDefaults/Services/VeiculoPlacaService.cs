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
            if (string.IsNullOrWhiteSpace(placa))
                return new VeiculoPlacaResponse { Placa = null, Acesso = false };

            var placaInput = placa.Trim().ToUpperInvariant();

            // carrega as placas do banco (pode otimizar depois)
            var veiculos = await _context.Veiculos
                .AsNoTracking()
                .Select(v => new { v.Placa })
                .ToListAsync();

            string? placaEncontrada = null;

            foreach (var v in veiculos)
            {
                if (string.IsNullOrWhiteSpace(v.Placa))
                    continue;

                var placaBanco = v.Placa.Trim().ToUpperInvariant();
                int iguais = 0;
                int maxCompare = Math.Min(placaBanco.Length, placaInput.Length);

                for (int i = 0; i < maxCompare; i++)
                {
                    if (placaBanco[i] == placaInput[i])
                        iguais++;
                }

                if (iguais >= 4)
                {
                    placaEncontrada = placaBanco;
                    break;
                }
            }

            return new VeiculoPlacaResponse
            {
                Placa = placaEncontrada,
                Acesso = placaEncontrada != null
            };
        }

        public async Task<TipoAcesso> GetProximoAcessoAsync(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return TipoAcesso.Entrada; // ou outra regra que prefira

            var placaInput = placa.Trim().ToUpperInvariant();

            // carrega as placas do banco (pode otimizar depois)
            var veiculos = await _context.Veiculos
                .AsNoTracking()
                .Select(v => new { v.Placa })
                .ToListAsync();

            string? placaEncontrada = null;

            foreach (var v in veiculos)
            {
                if (string.IsNullOrWhiteSpace(v.Placa))
                    continue;

                var placaBanco = v.Placa.Trim().ToUpperInvariant();
                int iguais = 0;
                int maxCompare = Math.Min(placaBanco.Length, placaInput.Length);

                for (int i = 0; i < maxCompare; i++)
                {
                    if (placaBanco[i] == placaInput[i])
                        iguais++;
                }

                if (iguais >= 4)
                {
                    placaEncontrada = placaBanco;
                    break;
                }
            }

            if (placaEncontrada == null)
                return TipoAcesso.Entrada; // sem placa parecida encontrada

            var ultimoLog = await _context.Logs
                .AsNoTracking()
                .Where(l => l.Placa == placaEncontrada)
                .OrderByDescending(l => l.DataHora)
                .FirstOrDefaultAsync();

            if (ultimoLog is null || ultimoLog.Tipo == TipoAcesso.Saida.ToString())
                return TipoAcesso.Entrada;

            return TipoAcesso.Saida;
        }
    }
}
