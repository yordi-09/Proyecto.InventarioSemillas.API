namespace Proyecto.InventarioSemillas.API.Models
{
    public class Ubicacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string CondicionesAlmacenamiento { get; set; } = string.Empty;

        public ICollection<Semilla>? Semillas { get; set; }
    }
}
