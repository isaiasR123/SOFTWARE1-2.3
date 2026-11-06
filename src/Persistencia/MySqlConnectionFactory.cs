using MySqlConnector;
using System.Data;

namespace Persistencia;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _cadenaConexion;

    // Recibimos la cadena directamente, sin IConfiguration
    public MySqlConnectionFactory(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public IDbConnection CrearConexionAdministrador()
    {
        var conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }

    public IDbConnection CrearConexionInvitado()
    {
        var conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }
}
