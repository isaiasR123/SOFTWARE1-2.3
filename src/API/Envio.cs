namespace API;

public sealed record SolicitudEnvio(
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino);

public sealed record Envio(
    int Id,
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino,
    string Estado,
    decimal Costo,
    int DiasEstimados);

public sealed record Cotizacion(decimal Costo, int DiasEstimados);

public sealed record SolicitudCambioEstado(string NuevoEstado);

public sealed record ErrorResponse(string Error);