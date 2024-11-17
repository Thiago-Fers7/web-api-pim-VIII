using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingAppApi.Data;
using Web_API.Models;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("/api/item-playlists")]
    public class ItemPlaylistsController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPlaylist>>> GetItemPlaylist()
        {
            var itemPlaylists = await _context.ItemPlaylists.ToListAsync();

            return Ok(itemPlaylists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPlaylist>> GetItemPlaylistById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemPlaylist = await _context.ItemPlaylists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (itemPlaylist == null)
            {
                return NotFound();
            }

            return Ok(itemPlaylist);
        }

        [HttpGet("all-playlist-items/{playlistId}")]
        public async Task<ActionResult<ItemPlaylist>> GetAllPlaylistItens(int? playlistId)
        {
            if (playlistId == null)
            {
                return NotFound();
            }

            var allPlaylistItems = await _context.ItemPlaylists
                .Where(m => m.PlaylistId == playlistId)
                .ToListAsync();

            if (allPlaylistItems.Count == 0)
            {
                return NotFound();
            }


            return Ok(allPlaylistItems);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemPlaylist([Bind("Id,PlaylistId,ConteudoId")] ItemPlaylist itemPlaylist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemPlaylist);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemPlaylist), new { id = itemPlaylist.Id }, itemPlaylist);
            }

            return Ok(itemPlaylist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemPlaylist(int id, [Bind("Id,PlaylistId,ConteudoId")] ItemPlaylist itemPlaylist)
        {
            if (id != itemPlaylist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemPlaylist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemPlaylistExists(itemPlaylist.Id))
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
            return Ok(itemPlaylist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemPlaylist(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemPlaylist = await _context.ItemPlaylists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemPlaylist == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        private bool ItemPlaylistExists(int id)
        {
            return _context.ItemPlaylists.Any(e => e.Id == id);
        }
    }
}
