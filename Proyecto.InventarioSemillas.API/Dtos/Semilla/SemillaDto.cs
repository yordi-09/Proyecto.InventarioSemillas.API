namespace Proyecto.InventarioSemillas.API.Dtos.Semilla
{
    public class SemillaDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public int EspecieId { get; set; }
        public int UbicacionId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAlmacenamiento { get; set; }
    }
}
