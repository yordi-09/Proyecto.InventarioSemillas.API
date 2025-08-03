namespace Proyecto.InventarioSemillas.API.Dtos.Especie
{
    public class EspecieDto
    {
        public int Id { get; set; }
        public required string NombreCientifico { get; set; }
        public required string NombreComun { get; set; }
        public required string Familia { get; set; }
        public required string Descripcion { get; set; }
    }
}
