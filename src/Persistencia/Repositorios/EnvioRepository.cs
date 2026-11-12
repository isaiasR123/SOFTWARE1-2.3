using Dapper;
using Persistencia.Entidades;

namespace Persistencia.Repositorios;

public class EnvioRepository : IEnvioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EnvioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public int Crear(Envio envio)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.QuerySingle<int>(
            """
            INSERT INTO Envio (Modalidad, PesoKg, DistanciaKm, Destino, Costo, DiasEstimados, Estado)
            VALUES (@Modalidad, @PesoKg, @DistanciaKm, @Destino, @Costo, @DiasEstimados, @Estado);
            SELECT LAST_INSERT_ID();
            """, envio);
    }

    public List<Envio> Listar()
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();
        return conexion.Query<Envio>("SELECT * FROM Envio ORDER BY Id DESC").ToList();
    }

    public Envio? Buscar(int id)
    {
        using var conexion = _connectionFactory.CrearConexionInvitado();
        return conexion.QuerySingleOrDefault<Envio>("SELECT * FROM Envio WHERE Id = @Id", new { Id = id });
    }

    public bool Actualizar(Envio envio)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.Execute(
            """
            UPDATE Envio SET
                Modalidad = @Modalidad, PesoKg = @PesoKg, DistanciaKm = @DistanciaKm,
                Destino = @Destino, Costo = @Costo, DiasEstimados = @DiasEstimados
            WHERE Id = @Id
            """, envio) > 0;
    }

    public bool Eliminar(int id)
    {
        using var conexion = _connectionFactory.CrearConexionAdministrador();
        return conexion.Execute("DELETE FROM Envio WHERE Id = @Id", new { Id = id }) > 0;
    }
}
