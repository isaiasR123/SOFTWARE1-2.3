namespace Persistencia.Entidades;

public sealed record EnvioApiRecord(
    int Id,
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino,
    string Estado);