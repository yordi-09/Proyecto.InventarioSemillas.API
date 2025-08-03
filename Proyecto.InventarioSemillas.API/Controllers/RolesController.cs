using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Dtos.Roles;

namespace Proyecto.InventarioSemillas.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(RoleManager<IdentityRole> roleManager, 
                                 UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        private readonly UserManager<IdentityUser> _userManager = userManager;

        /// <summary>
        /// Crea un nuevo rol en el sistema.
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificamos si el rol ya existe para no duplicarlo
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (roleExists)
            {
                return BadRequest($"El rol '{model.RoleName}' ya existe.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            if (result.Succeeded)
            {
                return Ok($"Rol '{model.RoleName}' creado exitosamente.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Obtiene una lista de todos los roles existentes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToListAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Elimina un rol por su nombre.
        /// </summary>
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound($"No se encontró el rol '{roleName}'.");
            }

            // Opcional: Verificar si hay usuarios en este rol antes de borrarlo
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole.Any())
            {
                return BadRequest($"No se puede eliminar el rol '{roleName}' porque tiene usuarios asignados.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok($"Rol '{roleName}' eliminado exitosamente.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Asigna un rol a un usuario específico.
        /// </summary>
        [HttpPost("asignar-rol")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound($"No se encontró el usuario con email '{model.Email}'.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return NotFound($"No se encontró el rol '{model.RoleName}'.");
            }

            // Evitar asignar un rol que el usuario ya tiene
            if (await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return BadRequest($"El usuario ya tiene el rol '{model.RoleName}'.");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return Ok($"Rol '{model.RoleName}' asignado al usuario '{model.Email}' exitosamente.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Remueve un rol de un usuario específico.
        /// </summary>
        [HttpPost("remover-rol")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound($"No se encontró el usuario con email '{model.Email}'.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return NotFound($"No se encontró el rol '{model.RoleName}'.");
            }

            // Verificar que el usuario realmente tenga ese rol antes de intentar removerlo
            if (!await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return BadRequest($"El usuario no tiene el rol '{model.RoleName}'.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return Ok($"Rol '{model.RoleName}' removido del usuario '{model.Email}' exitosamente.");
            }

            return BadRequest(result.Errors);
        }
    }
}
