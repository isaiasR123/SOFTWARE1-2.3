using Dapper;
using Persistencia;

namespace Aplicacion.Servicios;

public class EstadisticaService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EstadisticaService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<dynamic> ResumenEnviosPorPeriodo(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query(
            "ResumenEnviosPorPeriodo",
            new
            {
                p_FechaInicio = fechaInicio,
                p_FechaFin = fechaFin
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public IEnumerable<dynamic> CostosPorModalidad(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query(
            "CostosPorModalidad",
            new
            {
                p_FechaInicio = fechaInicio,
                p_FechaFin = fechaFin
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public IEnumerable<dynamic> ComparativaEstadosPorPeriodo(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query(
            "ComparativaEstadosPorPeriodo",
            new
            {
                p_FechaInicio = fechaInicio,
                p_FechaFin = fechaFin
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public IEnumerable<dynamic> TiempoPromedioEntrega(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query(
            "TiempoPromedioEntrega",
            new
            {
                p_FechaInicio = fechaInicio,
                p_FechaFin = fechaFin
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public IEnumerable<dynamic> FacturacionPorModalidad(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query(
            "FacturacionPorModalidad",
            new
            {
                p_FechaInicio = fechaInicio,
                p_FechaFin = fechaFin
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}