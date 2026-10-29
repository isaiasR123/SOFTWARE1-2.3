using Dapper;
using Persistencia.Entidades;

namespace Persistencia.Repositorios;

public class EnvioRepository : IEnvioApiRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EnvioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<T> ConsultarInvitado<T>(
        string sql,
        object? parametros = null)
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();

        return conexion.Query<T>(sql, parametros);
    }

    public T ConsultarPrimeroInvitado<T>(
        string sql,
        object parametros)
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();

        return conexion.QueryFirst<T>(sql, parametros);
    }

    public int EjecutarAdministrador(string sql, object parametros)
    {
        using var conexion =
            _connectionFactory.CrearConexionAdministrador();

        return conexion.Execute(sql, parametros);
    }

    public int Crear(EnvioApiRecord envio)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        conexion.Execute(
            """
            INSERT INTO EnvioApi (Modalidad, PesoKg, DistanciaKm, Destino, Estado)
            VALUES (@Modalidad, @PesoKg, @DistanciaKm, @Destino, @Estado);
            """,
            envio);

        return conexion.QuerySingle<int>("SELECT LAST_INSERT_ID();");
    }

    public IEnumerable<EnvioApiRecord> Listar()
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();
        return conexion.Query<EnvioApiRecord>(
            "SELECT Id, Modalidad, PesoKg, DistanciaKm, Destino, Estado FROM EnvioApi ORDER BY Id;");
    }

    public EnvioApiRecord? Buscar(int id)
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();
        return conexion.QuerySingleOrDefault<EnvioApiRecord>(
            "SELECT Id, Modalidad, PesoKg, DistanciaKm, Destino, Estado FROM EnvioApi WHERE Id = @Id;",
            new { Id = id });
    }

    public bool Actualizar(EnvioApiRecord envio)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.Execute(
            """
            UPDATE EnvioApi
            SET Modalidad = @Modalidad,
                PesoKg = @PesoKg,
                DistanciaKm = @DistanciaKm,
                Destino = @Destino
            WHERE Id = @Id;
            """,
            envio) > 0;
    }

    public bool CambiarEstado(int id, string estadoActual, string estadoNuevo)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.Execute(
            "UPDATE EnvioApi SET Estado = @EstadoNuevo WHERE Id = @Id AND Estado = @EstadoActual;",
            new { Id = id, EstadoActual = estadoActual, EstadoNuevo = estadoNuevo }) > 0;
    }

    public bool Eliminar(int id)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.Execute(
            "DELETE FROM EnvioApi WHERE Id = @Id;",
            new { Id = id }) > 0;
    }
}