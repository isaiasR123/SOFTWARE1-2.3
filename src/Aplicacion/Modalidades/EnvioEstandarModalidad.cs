namespace Aplicacion.Modalidades;

public sealed class EnvioEstandarModalidad : IEnvioModalidad
{
    public string Nombre => "estandar";
    public int DiasEstimados => 5;

    public decimal CalcularCosto(decimal pesoKg, decimal distanciaKm)
    {
        return 1000m + 200m * pesoKg + 10m * distanciaKm;
    }
}