using Dapper;
using Persistencia;
using Persistencia.Repositorios;
using System.Data;
using Persistencia.Entidades;
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
    Envio envio,
    int idModalidad)
{
    if (envio == null)
        throw new ArgumentException("El envío es obligatorio.");

    if (idModalidad <= 0)
        throw new ArgumentException("La modalidad no es válida.");

    double costo = envio.CalcularCosto();

    double tiempoEstimado =
        envio.CalcularTiempoEntrega();

    using var conexion =
        _connectionFactory.CrearConexionDesarrollo();

    conexion.Execute(
        "RegistrarEnvioCompleto",
        new
        {
            p_IdCliente = envio.Cliente.Id,
            p_IdPaquete = envio.Paquete.Id,
            p_IdModalidad = idModalidad,
            p_IdDireccionOrigen = envio.DireccionOrigen.Id,
            p_IdDireccionDestino = envio.DireccionDestino.Id,
            p_Distancia = envio.Distancia,
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