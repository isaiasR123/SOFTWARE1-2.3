using MySqlConnector;
using System.Data;

namespace Persistencia;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _cadenaAdmin;
    private readonly string _cadenaInvitado;

    public MySqlConnectionFactory(string cadenaAdmin, string cadenaInvitado)
    {
        _cadenaAdmin = cadenaAdmin;
        _cadenaInvitado = cadenaInvitado;
    }

    public IDbConnection CrearConexionAdministrador()
    {
        var conexion = new MySqlConnection(_cadenaAdmin);
        conexion.Open();
        return conexion;
    }

    public IDbConnection CrearConexionInvitado()
    {
        var conexion = new MySqlConnection(_cadenaInvitado);
        conexion.Open();
        return conexion;
    }
}
