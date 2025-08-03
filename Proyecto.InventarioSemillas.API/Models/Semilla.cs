namespace Proyecto.InventarioSemillas.API.Models
{
    public class Semilla
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int EspecieId { get; set; }
        public Especie? Especie { get; set; }
        public int UbicacionId { get; set; }
        public Ubicacion? Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAlmacenamiento { get; set; }
    }
}
