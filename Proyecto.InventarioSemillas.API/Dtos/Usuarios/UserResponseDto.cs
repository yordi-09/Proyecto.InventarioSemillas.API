namespace Proyecto.InventarioSemillas.API.Dtos.Usuarios
{
    public class UserResponseDto
    {
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}