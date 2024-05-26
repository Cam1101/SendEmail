using SendEmail.Models;

namespace SendEmail.Models
{
    public class EmailDTO
    {
        public string Para {  get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public List<ProductoDTO> Productos { get; set; } = new List<ProductoDTO>();
        public decimal TotalPago { get; set; }
        public DateTime FechaPedido { get; set; }
    }
}
