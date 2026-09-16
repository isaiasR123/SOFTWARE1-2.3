

using Persistencia;
using Persistencia.Repositorios;

string conexionAdministrador =
    "Server=localhost;Database=logistica;User ID=administrador;Password=Admin1234;";

string conexionDesarrollo =
    "Server=localhost;Database=logistica;User ID=desarrollo;Password=Desarrollo1234;";

IDbConnectionFactory factory =
    new MySqlConnectionFactory(
        conexionAdministrador,
        conexionDesarrollo);

EnvioRepository repositorio =
    new EnvioRepository(factory);

var envios = repositorio.Consultar<dynamic>(
    "SELECT * FROM Envio");

foreach (var envio in envios)
{
    Console.WriteLine(
        $"Envío: {envio.IdEnvio} - Costo: {envio.Costo}");
}