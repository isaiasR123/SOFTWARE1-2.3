namespace API.Dtos;

public record ActualizarEnvioDto(
    string Modalidad,
    decimal PesoKg,
    decimal DistanciaKm,
    string Destino
);
