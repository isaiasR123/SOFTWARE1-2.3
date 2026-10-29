namespace Aplicacion.Modalidades;

public interface IEnvioModalidad
{
    string Nombre { get; }
    int DiasEstimados { get; }
    decimal CalcularCosto(decimal pesoKg, decimal distanciaKm);
}