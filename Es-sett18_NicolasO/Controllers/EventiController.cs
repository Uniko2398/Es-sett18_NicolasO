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
    public class EventiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventi()
        {
            return await _context.Eventi
                .Include(e => e.Artista)
                .Select(e => new EventoDto
                {
                    EventoId = e.EventoId,
                    Titolo = e.Titolo,
                    Data = e.Data,
                    Luogo = e.Luogo,
                    ArtistaId = e.ArtistaId,
                    ArtistaNome = e.Artista.Nome
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var evento = await _context.Eventi
                .Include(e => e.Artista)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
            {
                return NotFound();
            }

            return new EventoDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                ArtistaId = evento.ArtistaId,
                ArtistaNome = evento.Artista.Nome
            };
        }

        [HttpPost]
        [Authorize(Roles = "Amministratore")]
        public async Task<ActionResult<EventoDto>> PostEvento(EventoCreateDto eventoDto)
        {
            var evento = new Evento
            {
                Titolo = eventoDto.Titolo,
                Data = eventoDto.Data,
                Luogo = eventoDto.Luogo,
                ArtistaId = eventoDto.ArtistaId
            };

            _context.Eventi.Add(evento);
            await _context.SaveChangesAsync();

            var artista = await _context.Artisti.FindAsync(evento.ArtistaId);

            return CreatedAtAction("GetEvento", new { id = evento.EventoId }, new EventoDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                ArtistaId = evento.ArtistaId,
                ArtistaNome = artista?.Nome ?? string.Empty
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> PutEvento(int id, EventoCreateDto eventoDto)
        {
            var evento = new Evento
            {
                EventoId = id,
                Titolo = eventoDto.Titolo,
                Data = eventoDto.Data,
                Luogo = eventoDto.Luogo,
                ArtistaId = eventoDto.ArtistaId
            };

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventi.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Eventi.Any(e => e.EventoId == id);
        }
    }
}
