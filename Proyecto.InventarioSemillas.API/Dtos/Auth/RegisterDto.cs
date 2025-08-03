namespace Proyecto.InventarioSemillas.API.Dtos.Auth
{
    public class RegisterDto
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }

        public string GetNombreCompleto()
        {
            return $"{Nombre}_{Apellido}";
        }
    }
}