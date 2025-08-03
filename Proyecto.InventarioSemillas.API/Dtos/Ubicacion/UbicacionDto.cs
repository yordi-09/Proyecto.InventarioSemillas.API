namespace Proyecto.InventarioSemillas.API.Dtos.Ubicacion
{
    public class UbicacionDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required string CondicionesAlmacenamiento { get; set; }
    }
}
