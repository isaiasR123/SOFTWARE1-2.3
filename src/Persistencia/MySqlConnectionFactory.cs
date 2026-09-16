using System.Data;
using MySqlConnector;

namespace Persistencia;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _conexionAdministrador;
    private readonly string _conexionDesarrollo;

    public MySqlConnectionFactory(
        string conexionAdministrador,
        string conexionDesarrollo)
    {
        _conexionAdministrador = conexionAdministrador;
        _conexionDesarrollo = conexionDesarrollo;
    }

    public IDbConnection CrearConexionAdministrador()
    {
        return new MySqlConnection(_conexionAdministrador);
    }

    public IDbConnection CrearConexionDesarrollo()
    {
        return new MySqlConnection(_conexionDesarrollo);
    }
}