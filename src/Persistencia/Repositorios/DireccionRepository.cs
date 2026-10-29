using Dapper;

namespace Persistencia.Repositorios;

public class DireccionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DireccionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<T> Consultar<T>(
        string sql,
        object? parametros = null)
    {
        using var conexion =
            _connectionFactory.CrearConexionInvitado();

        return conexion.Query<T>(sql, parametros);
    }

    public T ConsultarPrimero<T>(
        string sql,
        object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionInvitado();

        return conexion.QueryFirst<T>(sql, parametros);
    }

    public int Ejecutar(
        string sql,
        object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionAdministrador();

        return conexion.Execute(sql, parametros);
    }
}