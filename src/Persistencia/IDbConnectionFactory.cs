using System.Data;

namespace Persistencia;

public interface IDbConnectionFactory
{
    IDbConnection CrearConexionAdministrador();
    IDbConnection CrearConexionInvitado();
}
