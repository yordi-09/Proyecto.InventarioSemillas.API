using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Dtos.Usuarios;

namespace Proyecto.InventarioSemillas.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsuariosController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Obtiene todos los usuarios del sistema con sus roles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserResponseDto
                {
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Rol = roles.FirstOrDefault() ?? string.Empty
                });
            }

            return Ok(userDtos);
        }
    }
}
