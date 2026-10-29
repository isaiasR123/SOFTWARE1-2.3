using System.Data;
using MySqlConnector;

namespace Persistencia;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _conexionAdministrador;
    private readonly string _conexionInvitado;

    public MySqlConnectionFactory(
        string conexionAdministrador,
        string conexionInvitado)
    {
        _conexionAdministrador = conexionAdministrador;
        _conexionInvitado = conexionInvitado;
    }

    public static MySqlConnectionFactory DesdeVariablesEntorno()
    {
        return new MySqlConnectionFactory(
            CrearCadenaConexion("LOGISTICA_DB_ADMIN_USER", "LOGISTICA_DB_ADMIN_PASSWORD"),
            CrearCadenaConexion("LOGISTICA_DB_GUEST_USER", "LOGISTICA_DB_GUEST_PASSWORD"));
    }

    public IDbConnection CrearConexionAdministrador()
    {
        return new MySqlConnection(_conexionAdministrador);
    }

    public IDbConnection CrearConexionInvitado()
    {
        return new MySqlConnection(_conexionInvitado);
    }

    private static string CrearCadenaConexion(string variableUsuario, string variableContrasena)
    {
        static string RequerirVariable(string nombre)
        {
            return Environment.GetEnvironmentVariable(nombre) is { Length: > 0 } valor
                ? valor
                : throw new InvalidOperationException(
                    $"Falta configurar la variable de entorno {nombre}.");
        }

        return new MySqlConnectionStringBuilder
        {
            Server = Environment.GetEnvironmentVariable("LOGISTICA_DB_HOST") ?? "localhost",
            Port = uint.TryParse(Environment.GetEnvironmentVariable("LOGISTICA_DB_PORT"), out var puerto)
                ? puerto
                : 3306,
            Database = Environment.GetEnvironmentVariable("LOGISTICA_DB_NAME") ?? "logistica",
            UserID = RequerirVariable(variableUsuario),
            Password = RequerirVariable(variableContrasena)
        }.ConnectionString;
    }
}