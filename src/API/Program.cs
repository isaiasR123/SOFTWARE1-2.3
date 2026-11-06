using Aplicacion.Interfaces;
using Aplicacion.Modalidades;
using Aplicacion.Servicios;
using Persistencia;
using Persistencia.Repositorios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEnvioModalidad, EnvioEstandarModalidad>();
builder.Services.AddScoped<IEnvioModalidad, EnvioExpressModalidad>();
builder.Services.AddScoped<EnvioService>();
builder.Services.AddScoped<EnvioRepository>();

// Conexión a la base de datos
builder.Services.AddScoped<IDbConnectionFactory>(prov => 
    new MySqlConnectionFactory(builder.Configuration.GetConnectionString("Default")!)
);

// ✅ Cambiamos los puertos para no chocar
builder.WebHost.UseUrls("http://localhost:5002", "https://localhost:7002");


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();

// 1. POST /api/envios — Crear
app.MapPost("/api/envios", (CrearEnvioRequest req, EnvioService servicio) =>
{
    try
    {
        var envio = servicio.CrearEnvio(req.Modalidad, req.PesoKg, req.DistanciaKm, req.Destino);
        return Results.Created($"/api/envios/{envio.Id}", envio);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Mensaje = ex.Message });
    }
}).WithName("CrearEnvio");

// 2. GET /api/envios — Listar
app.MapGet("/api/envios", (EnvioService servicio) =>
{
    return Results.Ok(servicio.ListarTodos());
}).WithName("ListarEnvios");

// 3. GET /api/envios/{id} — Obtener por ID
app.MapGet("/api/envios/{id:int}", (int id, EnvioService servicio) =>
{
    var envio = servicio.BuscarPorId(id);
    return envio is not null ? Results.Ok(envio) : Results.NotFound(new { Mensaje = $"No existe el envío {id}" });
}).WithName("ObtenerEnvio");

// 4. GET /api/envios/{id}/cotizacion — Solo costo y días
app.MapGet("/api/envios/{id:int}/cotizacion", (int id, EnvioService servicio) =>
{
    try
    {
        var res = servicio.ObtenerCotizacion(id);
        return Results.Ok(new { Costo = res.costo, DiasEstimados = res.diasEstimados });
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { Mensaje = $"No existe el envío {id}" });
    }
}).WithName("CotizarEnvio");

// 5. PUT /api/envios/{id} — Actualizar
app.MapPut("/api/envios/{id:int}", (int id, ActualizarEnvioRequest req, EnvioService servicio) =>
{
    try
    {
        var act = servicio.Actualizar(id, req.Modalidad, req.PesoKg, req.DistanciaKm, req.Destino);
        return Results.Ok(act);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { Mensaje = $"No existe el envío {id}" });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Mensaje = ex.Message });
    }
}).WithName("ActualizarEnvio");

// 6. DELETE /api/envios/{id} — Eliminar
app.MapDelete("/api/envios/{id:int}", (int id, EnvioService servicio) =>
{
    return servicio.Eliminar(id) ? Results.NoContent() : Results.NotFound(new { Mensaje = $"No existe el envío {id}" });
}).WithName("EliminarEnvio");

app.Run();

public class CrearEnvioRequest
{
    public string Modalidad { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
    public decimal DistanciaKm { get; set; }
    public string Destino { get; set; } = string.Empty;
}

public class ActualizarEnvioRequest
{
    public string Modalidad { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
    public decimal DistanciaKm { get; set; }
    public string Destino { get; set; } = string.Empty;
}
