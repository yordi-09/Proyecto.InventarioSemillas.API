using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Dtos.Especie;
using Proyecto.InventarioSemillas.API.Models;

namespace Proyecto.InventarioSemillas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspeciesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EspecieDto>>> GetEspecies()
        {
            return await _context.Especies
                .Select(e => new EspecieDto
                {
                    Id = e.Id,
                    NombreCientifico = e.NombreCientifico,
                    NombreComun = e.NombreComun,
                    Familia = e.Familia,
                    Descripcion = e.Descripcion
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EspecieDto>> GetEspecie(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();

            return new EspecieDto
            {
                Id = especie.Id,
                NombreCientifico = especie.NombreCientifico,
                NombreComun = especie.NombreComun,
                Familia = especie.Familia,
                Descripcion = especie.Descripcion
            };
        }

        [HttpPost]
        public async Task<ActionResult> PostEspecie(EspecieDto dto)
        {
            var especie = new Especie
            {
                NombreCientifico = dto.NombreCientifico,
                NombreComun = dto.NombreComun,
                Familia = dto.Familia,
                Descripcion = dto.Descripcion
            };

            _context.Especies.Add(especie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEspecie), new { id = especie.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecie(int id, EspecieDto dto)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();

            especie.NombreCientifico = dto.NombreCientifico;
            especie.NombreComun = dto.NombreComun;
            especie.Familia = dto.Familia;
            especie.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecie(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();

            _context.Especies.Remove(especie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
