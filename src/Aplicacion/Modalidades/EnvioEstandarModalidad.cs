using Aplicacion.Interfaces;

namespace Aplicacion.Modalidades;

public sealed class EnvioEstandarModalidad : IEnvioModalidad
{
    public string Nombre => "estandar";
    public int DiasEstimados => 5;

    public decimal CalcularCosto(decimal pesoKg, decimal distanciaKm)
    {
        if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor que cero.");
        if (distanciaKm <= 0) throw new ArgumentException("La distancia debe ser mayor que cero.");
        return 1000 + 200 * pesoKg + 10 * distanciaKm;
    }
}
