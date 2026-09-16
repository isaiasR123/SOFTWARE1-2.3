using Dapper;
using Persistencia;
using Persistencia.Repositorios;
using System.Data;

namespace Aplicacion.Servicios;

public class EnvioService
{
    private readonly EnvioRepository _envioRepository;
    private readonly IDbConnectionFactory _connectionFactory;

    public EnvioService(
        EnvioRepository envioRepository,
        IDbConnectionFactory connectionFactory)
    {
        _envioRepository = envioRepository;
        _connectionFactory = connectionFactory;
    }

    // ==========================================
    // REGISTRAR ENVÍO
    // ==========================================

    public void RegistrarEnvio(
        int idCliente,
        int idPaquete,
        int idModalidad,
        int idDireccionOrigen,
        int idDireccionDestino,
        decimal distancia,
        decimal costo,
        int tiempoEstimado)
    {
        if (idCliente <= 0)
            throw new ArgumentException("El cliente no es válido.");

        if (idPaquete <= 0)
            throw new ArgumentException("El paquete no es válido.");

        if (idModalidad <= 0)
            throw new ArgumentException("La modalidad no es válida.");

        if (idDireccionOrigen <= 0)
            throw new ArgumentException("La dirección de origen no es válida.");

        if (idDireccionDestino <= 0)
            throw new ArgumentException("La dirección de destino no es válida.");

        if (distancia <= 0)
            throw new ArgumentException("La distancia debe ser mayor que cero.");

        if (costo < 0)
            throw new ArgumentException("El costo no puede ser negativo.");

        if (tiempoEstimado <= 0)
            throw new ArgumentException(
                "El tiempo estimado debe ser mayor que cero.");

        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        conexion.Execute(
            "RegistrarEnvioCompleto",
            new
            {
                p_IdCliente = idCliente,
                p_IdPaquete = idPaquete,
                p_IdModalidad = idModalidad,
                p_IdDireccionOrigen = idDireccionOrigen,
                p_IdDireccionDestino = idDireccionDestino,
                p_Distancia = distancia,
                p_Costo = costo,
                p_TiempoEstimado = tiempoEstimado
            },
            commandType: CommandType.StoredProcedure);
    }

    // ==========================================
    // RECUPERAR / LISTAR ENVÍOS
    // ==========================================

    public IEnumerable<dynamic> ListarEnvios()
    {
        return _envioRepository.ConsultarDesarrollo<dynamic>(
            "SELECT * FROM Envio");
    }

    // ==========================================
    // BUSCAR ENVÍO
    // ==========================================

    public dynamic BuscarEnvio(int idEnvio)
    {
        if (idEnvio <= 0)
            throw new ArgumentException("El ID del envío no es válido.");

        return _envioRepository.ConsultarPrimeroDesarrollo<dynamic>(
            "SELECT * FROM Envio WHERE IdEnvio = @IdEnvio",
            new { IdEnvio = idEnvio });
    }

    // ==========================================
    // CAMBIAR ESTADO
    // ==========================================

    public void CambiarEstado(
        int idEnvio,
        int idEstadoNuevo)
    {
        if (idEnvio <= 0)
            throw new ArgumentException("El ID del envío no es válido.");

        if (idEstadoNuevo <= 0)
            throw new ArgumentException("El estado no es válido.");

        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        conexion.Execute(
            "ActualizarEstadoEnvio",
            new
            {
                p_IdEnvio = idEnvio,
                p_IdEstadoNuevo = idEstadoNuevo
            },
            commandType: CommandType.StoredProcedure);
    }

    // ==========================================
    // CANCELAR ENVÍO
    // ==========================================

    public void CancelarEnvio(int idEnvio)
    {
        if (idEnvio <= 0)
            throw new ArgumentException("El ID del envío no es válido.");

        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        conexion.Execute(
            "CancelarEnvio",
            new
            {
                p_IdEnvio = idEnvio
            },
            commandType: CommandType.StoredProcedure);
    }

    // ==========================================
    // HISTORIAL DE ESTADOS
    // ==========================================

    public IEnumerable<dynamic> ConsultarHistorial(int idEnvio)
    {
        if (idEnvio <= 0)
            throw new ArgumentException("El ID del envío no es válido.");

        return _envioRepository.ConsultarDesarrollo<dynamic>(
            """
            SELECT
                h.IdHistorial,
                h.IdEnvio,
                e.Nombre AS Estado,
                h.FechaCambio
            FROM HistorialEstado h
            INNER JOIN Estado e
                ON h.IdEstado = e.IdEstado
            WHERE h.IdEnvio = @IdEnvio
            ORDER BY h.FechaCambio;
            """,
            new { IdEnvio = idEnvio });
    }
}