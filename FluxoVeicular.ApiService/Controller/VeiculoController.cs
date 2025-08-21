using FluxoVeicular.ServiceDefaults.Context;
using FluxoVeicular.ServiceDefaults.Entities;
using FluxoVeicular.ServiceDefaults.Requests;
using FluxoVeicular.ServiceDefaults.Responses;
using FluxoVeicular.ServiceDefaults.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluxoVeicular.ApiService.Controllers
{
    [ApiController]
    [Route("api/veiculo")]
    public class VeiculosController : ControllerBase
    {
        private readonly FluxoVeicularContext _context;
        private readonly VeiculoPlacaService _service;

        // IDE0290: Usar construtor primário (C# 12+)
        public VeiculosController(FluxoVeicularContext context, VeiculoPlacaService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/veiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoResponse>>> GetVeiculos()
        {
            var veiculos = await _context.Veiculos.ToListAsync();

            return veiculos.Select(v => new VeiculoResponse
            {
                Id = v.Id,
                Placa = v.Placa,
                Cor = v.Cor
            }).ToList();
        }

        // GET: api/veiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoResponse>> GetVeiculo(Guid id)
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

        // POST: api/veiculos
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

        // PUT: api/veiculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVeiculo(Guid id, VeiculoRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID do veículo não confere.");

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

        // DELETE: api/veiculos/5
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

        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<VeiculoPlacaResponse>> GetVeiculoPlaca(string placa)
        {
            var placaResponse = await _service.GetVeiculoByPlacaAsync(placa);

            return Ok(placaResponse); // sempre retorna o DTO
        }

    }
}
