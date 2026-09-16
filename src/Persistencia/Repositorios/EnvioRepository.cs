using Dapper;

namespace Persistencia.Repositorios;

public class EnvioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EnvioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // =========================
    // CONEXIÓN DESARROLLO
    // =========================

    public int EjecutarDesarrollo(string sql, object parametros)
    {
        using var conexion = _connectionFactory.CrearConexionDesarrollo();

        return conexion.Execute(sql, parametros);
    }

    public IEnumerable<T> ConsultarDesarrollo<T>(
        string sql,
        object? parametros = null)
    {
        using var conexion = _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query<T>(sql, parametros);
    }

    public T ConsultarPrimeroDesarrollo<T>(
        string sql,
        object parametros)
    {
        using var conexion = _connectionFactory.CrearConexionDesarrollo();

        return conexion.QueryFirst<T>(sql, parametros);
    }

    // =========================
    // CONEXIÓN ADMINISTRADOR
    // =========================

    public int EjecutarAdministrador(string sql, object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionAdministrador();

        return conexion.Execute(sql, parametros);
    }

    public IEnumerable<T> ConsultarAdministrador<T>(
        string sql,
        object? parametros = null)
    {
        using var conexion =
            _connectionFactory.CrearConexionAdministrador();

        return conexion.Query<T>(sql, parametros);
    }

    public T ConsultarPrimeroAdministrador<T>(
        string sql,
        object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionAdministrador();

        return conexion.QueryFirst<T>(sql, parametros);
    }
}