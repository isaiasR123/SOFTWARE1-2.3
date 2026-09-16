using Aplicacion.Servicios;
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

EnvioRepository envioRepository =
new EnvioRepository(factory);

EnvioService envioService =
new EnvioService(envioRepository, factory);

EstadisticaService estadisticaService =
new EstadisticaService(factory);

Console.WriteLine("Servicios de logística configurados correctamente.");
