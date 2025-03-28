using Es_sett18_NicolasO.Data;
using Es_sett18_NicolasO.DTOs;
using Es_sett18_NicolasO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Es_sett18_NicolasO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BigliettiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BigliettiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BigliettoDto>>> GetBiglietti()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("Amministratore"))
            {
                return await _context.Biglietti
                    .Include(b => b.Evento)
                    .ThenInclude(e => e.Artista)
                    .Select(b => new BigliettoDto
                    {
                        BigliettoId = b.BigliettoId,
                        DataAcquisto = b.DataAcquisto,
                        EventoId = b.EventoId,
                        EventoTitolo = b.Evento.Titolo,
                        EventoData = b.Evento.Data,
                        EventoLuogo = b.Evento.Luogo,
                        ArtistaNome = b.Evento.Artista.Nome
                    })
                    .ToListAsync();
            }
            else
            {
                return await _context.Biglietti
                    .Where(b => b.UserId == userId)
                    .Include(b => b.Evento)
                    .ThenInclude(e => e.Artista)
                    .Select(b => new BigliettoDto
                    {
                        BigliettoId = b.BigliettoId,
                        DataAcquisto = b.DataAcquisto,
                        EventoId = b.EventoId,
                        EventoTitolo = b.Evento.Titolo,
                        EventoData = b.Evento.Data,
                        EventoLuogo = b.Evento.Luogo,
                        ArtistaNome = b.Evento.Artista.Nome
                    })
                    .ToListAsync();
            }
        }

        [HttpPost]
        public async Task<ActionResult<BigliettoDto>> PostBiglietto(AcquistoBigliettoDto acquistoDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var evento = await _context.Eventi.FindAsync(acquistoDto.EventoId);

            if (evento == null)
            {
                return NotFound("Evento non trovato");
            }

            var biglietti = new List<Biglietto>();
            for (int i = 0; i < acquistoDto.Quantita; i++)
            {
                biglietti.Add(new Biglietto
                {
                    EventoId = acquistoDto.EventoId,
                    UserId = userId,
                    DataAcquisto = DateTime.Now
                });
            }

            _context.Biglietti.AddRange(biglietti);
            await _context.SaveChangesAsync();

            var firstBiglietto = biglietti.First();
            return CreatedAtAction("GetBiglietto", new { id = firstBiglietto.BigliettoId }, new BigliettoDto
            {
                BigliettoId = firstBiglietto.BigliettoId,
                DataAcquisto = firstBiglietto.DataAcquisto,
                EventoId = firstBiglietto.EventoId,
                EventoTitolo = evento.Titolo,
                EventoData = evento.Data,
                EventoLuogo = evento.Luogo
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BigliettoDto>> GetBiglietto(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var biglietto = await _context.Biglietti
                .Include(b => b.Evento)
                .ThenInclude(e => e.Artista)
                .FirstOrDefaultAsync(b => b.BigliettoId == id);

            if (biglietto == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Amministratore") && biglietto.UserId != userId)
            {
                return Forbid();
            }

            return new BigliettoDto
            {
                BigliettoId = biglietto.BigliettoId,
                DataAcquisto = biglietto.DataAcquisto,
                EventoId = biglietto.EventoId,
                EventoTitolo = biglietto.Evento.Titolo,
                EventoData = biglietto.Evento.Data,
                EventoLuogo = biglietto.Evento.Luogo,
                ArtistaNome = biglietto.Evento.Artista.Nome
            };
        }
    }
}
