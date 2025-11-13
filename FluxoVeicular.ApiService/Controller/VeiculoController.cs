using FluxoVeicular.App.Client.Enum;
using FluxoVeicular.App.Client.Request;
using FluxoVeicular.App.Client.Response;
using FluxoVeicular.ServiceDefaults.Context;
using FluxoVeicular.ServiceDefaults.Entities;
using FluxoVeicular.ServiceDefaults.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace FluxoVeicular.ApiService.Controller
{
    [ApiController]
    [Route("api/veiculos")]
    public class VeiculosController : ControllerBase
    {
        private readonly FluxoVeicularContext _context;
        private readonly VeiculoPlacaService _service;
        private readonly IHubContext<NotificacaoHub> _hub;
        public VeiculosController(FluxoVeicularContext context, VeiculoPlacaService service, IHubContext<NotificacaoHub> hub)
        {
            _context = context;
            _service = service;
            _hub = hub;
        }

        // pesquisa todos os veiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoResponse>>> GetVeiculo()
        {
            var veiculos = await _context.Veiculos.ToListAsync();

            return veiculos.Select(v => new VeiculoResponse
            {
                Id = v.Id,
                Placa = v.Placa,
                Cor = v.Cor
            }).ToList();
        }

        // pesquisa por id
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoResponse>> GetVeiculoById(Guid id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            return new VeiculoResponse
            {
                Id = veiculo.Id,
                Placa = veiculo.Placa,
                Cor = veiculo.Cor
            };
        }

        // adicionar veiculo
        [HttpPost]
        public async Task<ActionResult<VeiculoResponse>> CreateVeiculo(VeiculoRequest request)
        {
            var veiculo = new Veiculo
            {
                Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
                Placa = request.Placa,
                Cor = request.Cor
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            var response = new VeiculoResponse
            {
                Id = veiculo.Id,
                Placa = veiculo.Placa,
                Cor = veiculo.Cor
            };

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, response);
        }

        // atualiza veiculo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVeiculo(Guid id, VeiculoRequest request)
        {
            if (id != Guid.Empty)
            {
                var veiculo = await _context.Veiculos.FindAsync(id);
                if (veiculo == null)
                    return NotFound();

                veiculo.Placa = request.Placa;
                veiculo.Cor = request.Cor;

                _context.Entry(veiculo).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Veiculos.AnyAsync(v => v.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                return NoContent();
            }
            else
            {
                return BadRequest("ID inválido.");
            }
        }
        // excluir veiculo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(Guid id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Pesquisa por placa
        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<VeiculoPlacaResponse>> GetVeiculoPlaca(string placa)
        {
            var placaResponse = await _service.GetVeiculoByPlacaAsync(placa);
            var proximoAcesso = await _service.GetProximoAcessoAsync(placa);

            // Usa a placa encontrada (ou a original, se não achou)
            var placaFinal = placaResponse?.Placa ?? placa;

            // Grava log com a placa final
            await LogVeiculo(new LogRequest
            {
                Placa = placaFinal,
                DataHora = DateTime.UtcNow,
                Tipo = proximoAcesso.ToString()
            });

            // Envia alerta com a placa final
            var dados = placaResponse?.Placa is not null ? 1 : 2;

            await _hub.Clients.All.SendAsync("AlertaPlaca", new
            {
                Dados = dados,
                Mensagem = placaFinal
            });

            if (placaResponse is null)
                return NotFound($"Veículo com a placa {placa} não encontrado.");

            return Ok(placaResponse);
        }

        [HttpPost("log")]
        public async Task<ActionResult> LogVeiculo(LogRequest request)
        {
            var log = new Log
            {
                Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
                Placa = request.Placa,
                DataHora = request.DataHora,
                Tipo = request.Tipo
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}