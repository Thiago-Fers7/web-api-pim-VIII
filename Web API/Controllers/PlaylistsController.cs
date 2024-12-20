﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingAppApi.Data;
using Web_API.Models;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("/api/playlists")]
    public class PlaylistsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            return await _context.Playlists.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetPlaylistById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        [Route("user/{userId}")]
        [HttpGet]
        public async Task<ActionResult<Playlist>> GetPlaylistByUser(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var playlists = await _context.Playlists
                .Where(m => m.UsuarioId == userId)
                .ToListAsync();

            if (playlists.Count == 0)
            {
                return NotFound();
            }

            return Ok(playlists);
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist([Bind("Id,Nome,UsuarioId")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playlist);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] Playlist updatedPlaylist)
        {
            if (id != updatedPlaylist.Id)
            {
                return BadRequest("O ID da playlist na rota não corresponde ao ID no corpo da solicitação.");
            }

            var existingPlaylist = await _context.Playlists.FindAsync(id);
            if (existingPlaylist == null)
            {
                return NotFound($"A playlist com ID {id} não foi encontrada.");
            }

            existingPlaylist.Nome = updatedPlaylist.Nome;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(existingPlaylist);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);

            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
