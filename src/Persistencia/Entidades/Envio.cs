namespace Persistencia.Entidades;

public class Envio
{
    public int Id { get; set; }
    public string Modalidad { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
    public decimal DistanciaKm { get; set; }
    public string Destino { get; set; } = string.Empty;
    public decimal Costo { get; set; }
    public int DiasEstimados { get; set; }
    public string Estado { get; set; } = "PENDIENTE";
}
