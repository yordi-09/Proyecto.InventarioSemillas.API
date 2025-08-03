using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Dtos.Semilla;
using Proyecto.InventarioSemillas.API.Models;

namespace Proyecto.InventarioSemillas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemillasController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemillaDto>>> GetSemillas()
        {
            return await _context.Semillas
                .Select(s => new SemillaDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    EspecieId = s.EspecieId,
                    UbicacionId = s.UbicacionId,
                    Cantidad = s.Cantidad,
                    FechaAlmacenamiento = s.FechaAlmacenamiento
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SemillaDto>> GetSemilla(int id)
        {
            var semilla = await _context.Semillas.FindAsync(id);
            if (semilla == null) return NotFound();

            return new SemillaDto
            {
                Id = semilla.Id,
                Nombre = semilla.Nombre,
                EspecieId = semilla.EspecieId,
                UbicacionId = semilla.UbicacionId,
                Cantidad = semilla.Cantidad,
                FechaAlmacenamiento = semilla.FechaAlmacenamiento
            };
        }

        [HttpPost]
        public async Task<ActionResult> PostSemilla(SemillaDto dto)
        {
            // Validación: evitar duplicado por nombre
            if (await _context.Semillas.AnyAsync(s => s.Nombre.ToLower() == dto.Nombre.ToLower()))
                return BadRequest("Ya existe una semilla con ese nombre.");

            // Validación: existencia de especie y ubicación
            if (!await _context.Especies.AnyAsync(e => e.Id == dto.EspecieId))
                return BadRequest("Especie no válida.");

            if (!await _context.Ubicaciones.AnyAsync(u => u.Id == dto.UbicacionId))
                return BadRequest("Ubicación no válida.");

            var nueva = new Semilla
            {
                Nombre = dto.Nombre,
                EspecieId = dto.EspecieId,
                UbicacionId = dto.UbicacionId,
                Cantidad = dto.Cantidad,
                FechaAlmacenamiento = dto.FechaAlmacenamiento
            };

            _context.Semillas.Add(nueva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSemilla), new { id = nueva.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemilla(int id, SemillaDto dto)
        {
            var semilla = await _context.Semillas.FindAsync(id);
            if (semilla == null) return NotFound();

            // Validación: existencia de especie y ubicación
            if (!await _context.Especies.AnyAsync(e => e.Id == dto.EspecieId))
                return BadRequest("Especie no válida.");

            if (!await _context.Ubicaciones.AnyAsync(u => u.Id == dto.UbicacionId))
                return BadRequest("Ubicación no válida.");

            semilla.Nombre = dto.Nombre;
            semilla.EspecieId = dto.EspecieId;
            semilla.UbicacionId = dto.UbicacionId;
            semilla.Cantidad = dto.Cantidad;
            semilla.FechaAlmacenamiento = dto.FechaAlmacenamiento;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemilla(int id)
        {
            var semilla = await _context.Semillas.FindAsync(id);
            if (semilla == null) return NotFound();

            _context.Semillas.Remove(semilla);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
