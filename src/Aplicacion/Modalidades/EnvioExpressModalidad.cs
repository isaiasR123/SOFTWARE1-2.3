namespace Aplicacion.Modalidades;

public sealed class EnvioExpressModalidad : IEnvioModalidad
{
    public string Nombre => "express";
    public int DiasEstimados => 2;

    public decimal CalcularCosto(decimal pesoKg, decimal distanciaKm)
    {
        return 2000m + 350m * pesoKg + 20m * distanciaKm;
    }
}