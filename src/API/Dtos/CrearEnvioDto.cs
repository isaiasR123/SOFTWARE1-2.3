namespace API.Dtos;

public record CrearEnvioDto(
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino
);
