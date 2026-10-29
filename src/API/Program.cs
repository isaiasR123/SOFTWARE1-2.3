using API;
using Aplicacion.Modalidades;
using Persistencia;
using Persistencia.Repositorios;

var builder = WebApplication.CreateBuilder(args);

var connectionFactory = MySqlConnectionFactory.DesdeVariablesEntorno();

builder.Services.AddSingleton<IEnvioModalidad, EnvioEstandarModalidad>();
builder.Services.AddSingleton<IEnvioModalidad, EnvioExpressModalidad>();
builder.Services.AddSingleton<IDbConnectionFactory>(connectionFactory);
builder.Services.AddSingleton<IEnvioApiRepository, EnvioRepository>();
builder.Services.AddSingleton<ApiEnvioService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

var envios = app.MapGroup("/api/envios");

envios.MapPost("", (SolicitudEnvio solicitud, ApiEnvioService servicio) =>
{
    try
    {
        var envio = servicio.Crear(solicitud);
        return Results.Created($"/api/envios/{envio.Id}", envio);
    }
    catch (ArgumentException exception)
    {
        return Results.BadRequest(new ErrorResponse(exception.Message));
    }
})
    .WithSummary("Crear un envío")
    .WithDescription("Recibe modalidad (estandar o express), pesoKg, distanciaKm y destino.\n\n" +
        "Válido: `{ \"modalidad\": \"estandar\", \"pesoKg\": 5, \"distanciaKm\": 10, \"destino\": \"Córdoba\" }`.\n\n" +
        "Inválido: pesoKg igual a 0 o modalidad distinta de estandar/express.")
    .Produces<Envio>(StatusCodes.Status201Created)
    .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

envios.MapGet("", (ApiEnvioService servicio) => Results.Ok(servicio.Listar()))
    .WithSummary("Listar envíos")
    .WithDescription("No recibe parámetros. Devuelve todos los envíos o una lista vacía.")
    .Produces<IReadOnlyCollection<Envio>>(StatusCodes.Status200OK);

envios.MapGet("/{id:int}", (int id, ApiEnvioService servicio) =>
{
    var envio = servicio.Buscar(id);
    return envio is null ? Results.NotFound() : Results.Ok(envio);
})
    .WithSummary("Obtener un envío")
    .WithDescription("Recibe el identificador entero del envío en la ruta.")
    .Produces<Envio>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

envios.MapGet("/{id:int}/cotizacion", (int id, ApiEnvioService servicio) =>
{
    var envio = servicio.Buscar(id);
    return envio is null
        ? Results.NotFound()
        : Results.Ok(new Cotizacion(envio.Costo, envio.DiasEstimados));
})
    .WithSummary("Consultar cotización")
    .WithDescription("Recibe el identificador entero del envío en la ruta y devuelve costo y diasEstimados.")
    .Produces<Cotizacion>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

envios.MapPut("/{id:int}", (int id, SolicitudEnvio solicitud, ApiEnvioService servicio) =>
{
    try
    {
        var envio = servicio.Actualizar(id, solicitud);
        return envio is null ? Results.NotFound() : Results.Ok(envio);
    }
    catch (ArgumentException exception)
    {
        return Results.BadRequest(new ErrorResponse(exception.Message));
    }
})
    .WithSummary("Reemplazar los datos de un envío")
    .WithDescription("Recibe el identificador en la ruta y modalidad, pesoKg, distanciaKm y destino en el cuerpo. Conserva el ID y el estado.\n\n" +
        "Válido: `{ \"modalidad\": \"express\", \"pesoKg\": 2, \"distanciaKm\": 20, \"destino\": \"Rosario\" }`.\n\n" +
        "Inválido: `{ \"modalidad\": \"express\", \"pesoKg\": 0, \"distanciaKm\": 20, \"destino\": \"Rosario\" }` devuelve 400; un ID inexistente devuelve 404.")
    .Produces<Envio>(StatusCodes.Status200OK)
    .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

envios.MapDelete("/{id:int}", (int id, ApiEnvioService servicio) =>
    servicio.Eliminar(id) ? Results.NoContent() : Results.NotFound())
    .WithSummary("Eliminar un envío")
    .WithDescription("Recibe el identificador entero del envío en la ruta.\n\n" +
        "Válido: `DELETE /api/envios/1` si existe.\n\n" +
        "Inválido: `DELETE /api/envios/999` si no existe.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

envios.MapPatch("/{id:int}/estado", (int id, SolicitudCambioEstado solicitud, ApiEnvioService servicio) =>
{
    try
    {
        var envio = servicio.CambiarEstado(id, solicitud.NuevoEstado);
        return envio is null ? Results.NotFound() : Results.Ok(envio);
    }
    catch (ArgumentException exception)
    {
        return Results.BadRequest(new ErrorResponse(exception.Message));
    }
})
    .WithSummary("Avanzar el estado de un envío")
    .WithDescription("Recibe el ID en la ruta y nuevoEstado en el cuerpo. Solo permite PENDIENTE a DESPACHADO y DESPACHADO a ENTREGADO.\n\n" +
        "Válido: `{ \"nuevoEstado\": \"DESPACHADO\" }` para un envío PENDIENTE.\n\n" +
        "Inválido: `{ \"nuevoEstado\": \"ENTREGADO\" }` para un envío PENDIENTE devuelve 400. Un ID inexistente devuelve 404.")
    .Produces<Envio>(StatusCodes.Status200OK)
    .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

app.Run();