namespace API.Dtos;

public record EnvioRespuestaDto(
    int Id,
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino,
    decimal Costo,
    int DiasEstimados,
    string Estado
);
