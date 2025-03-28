using Es_sett18_NicolasO.Data;
using Es_sett18_NicolasO.DTOs;
using Es_sett18_NicolasO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Es_sett18_NicolasO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Amministratore")]
    public class ArtistiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistaDto>>> GetArtisti()
        {
            return await _context.Artisti
                .Select(a => new ArtistaDto
                {
                    ArtistaId = a.ArtistaId,
                    Nome = a.Nome,
                    Genere = a.Genere,
                    Biografia = a.Biografia
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistaDto>> GetArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);

            if (artista == null)
            {
                return NotFound();
            }

            return new ArtistaDto
            {
                ArtistaId = artista.ArtistaId,
                Nome = artista.Nome,
                Genere = artista.Genere,
                Biografia = artista.Biografia
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtista(int id, ArtistaCreateDto artistaDto)
        {
            var artista = await _context.Artisti.FindAsync(id);

            if (artista == null)
            {
                return NotFound();
            }

            artista.Nome = artistaDto.Nome;
            artista.Genere = artistaDto.Genere;
            artista.Biografia = artistaDto.Biografia;

            _context.Entry(artista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ArtistaDto>> PostArtista(ArtistaCreateDto artistaDto)
        {
            var artista = new Artista
            {
                Nome = artistaDto.Nome,
                Genere = artistaDto.Genere,
                Biografia = artistaDto.Biografia
            };

            _context.Artisti.Add(artista);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtista", new { id = artista.ArtistaId }, new ArtistaDto
            {
                ArtistaId = artista.ArtistaId,
                Nome = artista.Nome,
                Genere = artista.Genere,
                Biografia = artista.Biografia
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);
            if (artista == null)
            {
                return NotFound();
            }

            _context.Artisti.Remove(artista);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistaExists(int id)
        {
            return _context.Artisti.Any(e => e.ArtistaId == id);
        }
    }
}
