using Dapper;

namespace Persistencia.Repositorios;

public class PaqueteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PaqueteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<T> Consultar<T>(
        string sql,
        object? parametros = null)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Query<T>(sql, parametros);
    }

    public T ConsultarPrimero<T>(
        string sql,
        object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.QueryFirst<T>(sql, parametros);
    }

    public int Ejecutar(
        string sql,
        object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionDesarrollo();

        return conexion.Execute(sql, parametros);
    }
}