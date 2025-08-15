using FluxoVeicular.ServiceDefaults.Context;
using FluxoVeicular.ServiceDefaults.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluxoVeicular.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly FluxoVeicularContext _context;

        public VeiculosController(FluxoVeicularContext context)
        {
            _context = context;
        }

        // GET: api/veiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos.ToListAsync();
        }

        // GET: api/veiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(string id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            return veiculo;
        }

        // POST: api/veiculos
        [HttpPost]
        public async Task<ActionResult<Veiculo>> CreateVeiculo(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, veiculo);
        }

        // PUT: api/veiculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVeiculo(string id, Veiculo veiculo)
        {
            //if (id is null veiculo.Id)
            //    return BadRequest();

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Veiculos.AnyAsync(v => v.Id.ToString() == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/veiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(string id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
