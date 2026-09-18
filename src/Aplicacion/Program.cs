using Aplicacion.Servicios;
using Persistencia;
using Persistencia.Repositorios;

string password =
    Environment.GetEnvironmentVariable("LOGISTICA_DB_PASSWORD") ?? "";

string conexionAdministrador =
    $"Server=localhost;Database=logistica;User ID=5to_agbd;Password={password};";

string conexionDesarrollo =
    $"Server=localhost;Database=logistica;User ID=5to_agbd;Password={password};";

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

var envios = envioService.ListarEnvios();

Console.WriteLine("Cantidad de envíos: " + envios.Count());

DateTime fechaInicio =
    new DateTime(2026, 9, 18, 0, 0, 0);

DateTime fechaFin =
    new DateTime(2026, 9, 18, 23, 59, 59);

var resumen =
    estadisticaService.ResumenEnviosPorPeriodo(
        fechaInicio,
        fechaFin);

foreach (var item in resumen)
{
    Console.WriteLine(
        item.Modalidad + " | " +
        item.Estado + " | " +
        item.CantidadEnvios + " | " +
        item.CostoTotal);
}

Console.WriteLine();

Console.WriteLine("=== COSTOS POR MODALIDAD ===");

var costos =
    estadisticaService.CostosPorModalidad(
        fechaInicio,
        fechaFin);

foreach (var item in costos)
{
    Console.WriteLine(
        item.Modalidad + " | " +
        item.CostoAcumulado + " | " +
        item.CostoPromedio);
}

Console.WriteLine();

Console.WriteLine("=== COMPARATIVA DE ESTADOS ===");

var comparativa =
    estadisticaService.ComparativaEstadosPorPeriodo(
        fechaInicio,
        fechaFin);

foreach (var item in comparativa)
{
    Console.WriteLine(
        item.Estado + " | " +
        item.CantidadEnvios);
}

Console.WriteLine();

Console.WriteLine("=== TIEMPO PROMEDIO DE ENTREGA ===");

var tiempo =
    estadisticaService.TiempoPromedioEntrega(
        fechaInicio,
        fechaFin);

foreach (var item in tiempo)
{
    Console.WriteLine(
        item.Modalidad + " | " +
        item.TiempoPromedioEntrega);
}

Console.WriteLine();

Console.WriteLine("=== FACTURACION POR MODALIDAD ===");

var facturacion =
    estadisticaService.FacturacionPorModalidad(
        fechaInicio,
        fechaFin);

foreach (var item in facturacion)
{
    Console.WriteLine(
        item.Modalidad + " | " +
        item.FacturacionTotal);
}