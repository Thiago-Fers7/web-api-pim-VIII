using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingAppApi.Data;
using Web_API.Models;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("/api/criadores")]
    public class CriadoresController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Criador>>> GetCriadores()
        {
            return await _context.Criadores.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Criador>> PostCriador(Criador Criador)
        {
            _context.Criadores.Add(Criador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCriadores), new { id = Criador.Id }, Criador);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Criador>> DeleteCriador(int id)
        {
            var Criador = await _context.Criadores.FindAsync(id);

            if (Criador == null)
            {
                return NotFound();
            }

            _context.Criadores.Remove(Criador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
