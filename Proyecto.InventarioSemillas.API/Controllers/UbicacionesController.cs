using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Dtos.Ubicacion;
using Proyecto.InventarioSemillas.API.Models;

namespace Proyecto.InventarioSemillas.API.Controllers
{
    [Authorize(Roles = "Usuario")]
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UbicacionDto>>> GetUbicaciones()
        {
            return await _context.Ubicaciones
                .Select(u => new UbicacionDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    CondicionesAlmacenamiento = u.CondicionesAlmacenamiento
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UbicacionDto>> GetUbicacion(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null) return NotFound();

            return new UbicacionDto
            {
                Id = ubicacion.Id,
                Nombre = ubicacion.Nombre,
                Descripcion = ubicacion.Descripcion,
                CondicionesAlmacenamiento = ubicacion.CondicionesAlmacenamiento
            };
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostUbicacion(UbicacionDto dto)
        {
            // Validación: evitar nombre duplicado
            if (await _context.Ubicaciones.AnyAsync(u => u.Nombre.ToLower() == dto.Nombre.ToLower()))
                return false;

            var nueva = new Ubicacion
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CondicionesAlmacenamiento = dto.CondicionesAlmacenamiento
            };

            _context.Ubicaciones.Add(nueva);
            await _context.SaveChangesAsync();

            return true;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, UbicacionDto dto)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null) return NotFound();

            ubicacion.Nombre = dto.Nombre;
            ubicacion.Descripcion = dto.Descripcion;
            ubicacion.CondicionesAlmacenamiento = dto.CondicionesAlmacenamiento;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbicacion(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null) return NotFound();

            _context.Ubicaciones.Remove(ubicacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
