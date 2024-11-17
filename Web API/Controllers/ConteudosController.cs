using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingAppApi.Data;
using System;
using System.Text.Json;
using Web_API.Models;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("/api/conteudos")]
    public class ConteudosController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conteudo>>> GetConteudo()
        {
            var conteudos = await _context.Conteudos.ToListAsync();

            return Ok(conteudos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConteudoById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conteudo = await _context.Conteudos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (conteudo == null)
            {
                return NotFound();
            }

            return Ok(conteudo);
        }

        [HttpGet("conteudos-do-criador/{criadorId}")]
        public async Task<ActionResult<ConteudoResponse>> GetConteudoByCriador(int? criadorId)
        {
            if (criadorId == null)
            {
                return NotFound();
            }

            var conteudos = await _context.Conteudos
                .Where(m => m.CriadorId == criadorId)
                .ToListAsync();

            if (conteudos.Count == 0)
            {
                return NotFound();
            }

            var conteudoResponse = new List<ConteudoResponse>();

            // para cada conteúdo quero retornar em quantas playlists ele está
            foreach (var conteudo in conteudos)
            {
                var teste = await _context.ItemPlaylists
                    .Where(m => m.ConteudoId == conteudo.Id)
                    .Select(m => m.PlaylistId)
                    .ToListAsync();

                // adiciona o conteúdo com a quantidade de playlists que ele está
                conteudoResponse.Add(new ConteudoResponse
                {
                    CriadorId = conteudo.CriadorId,
                    Id = conteudo.Id,
                    Tipo = conteudo.Tipo,
                    Titulo = conteudo.Titulo,
                    PlaylistsCount = teste.Count,
                });
            }

            return Ok(conteudoResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConteudo([Bind("Titulo,Tipo,CriadorId")] Conteudo conteudo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conteudo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetConteudo), new { id = conteudo.Id }, conteudo);
            }

            return Ok(conteudo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConteudo(int id, [Bind("Titulo,Tipo,CriadorId")] Conteudo conteudo)
        {
            if (id != conteudo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conteudo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConteudoExists(conteudo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conteudo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Conteudo>> DeleteCriador(int id)
        {
            var Conteudo = await _context.Conteudos.FindAsync(id);

            if (Conteudo == null)
            {
                return NotFound();
            }

            _context.Conteudos.Remove(Conteudo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConteudoExists(int id)
        {
            return _context.Conteudos.Any(e => e.Id == id);
        }
    }
}
