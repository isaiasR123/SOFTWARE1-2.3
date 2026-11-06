using Aplicacion.Interfaces;

namespace Aplicacion.Modalidades;

public sealed class EnvioExpressModalidad : IEnvioModalidad
{
    public string Nombre => "express";
    public int DiasEstimados => 2;

    public decimal CalcularCosto(decimal pesoKg, decimal distanciaKm)
    {
        if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor que cero.");
        if (distanciaKm <= 0) throw new ArgumentException("La distancia debe ser mayor que cero.");
        return 2000 + 350 * pesoKg + 20 * distanciaKm;
    }
}
