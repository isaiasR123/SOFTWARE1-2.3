using Aplicacion.Interfaces;
using Aplicacion.Modalidades;
using Aplicacion.Servicios;
using API.Dtos;
using Persistencia;
using Persistencia.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// ✅ Servicios de la aplicación
builder.Services.AddScoped<IEnvioModalidad, EnvioEstandarModalidad>();
builder.Services.AddScoped<IEnvioModalidad, EnvioExpressModalidad>();
builder.Services.AddScoped<EnvioService>();
builder.Services.AddScoped<IEnvioRepository, EnvioRepository>();

// ✅ Conexiones a la base
var cadenaAdmin = builder.Configuration.GetValue<string>("ConnectionStrings:Administrador") 
    ?? throw new InvalidOperationException("Falta cadena Administrador en appsettings.json");
var cadenaInvitado = builder.Configuration.GetValue<string>("ConnectionStrings:Invitado") 
    ?? throw new InvalidOperationException("Falta cadena Invitado en appsettings.json");

builder.Services.AddScoped<IDbConnectionFactory>(_ => 
    new MySqlConnectionFactory(cadenaAdmin, cadenaInvitado));

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ==========================================
// 📬 LOS 7 ENDPOINTS — adaptados a TU código
// ==========================================

// ✅ POST /api/envios — Crear envío
app.MapPost("/api/envios", (CrearEnvioDto dto, EnvioService servicio) =>
{
    try
    {
        var envio = servicio.CrearEnvio(dto.Modalidad, dto.PesoKg, dto.DistanciaKm, dto.Destino);
        return Results.Created($"/api/envios/{envio.Id}", 
            new EnvioRespuestaDto(
                envio.Id,
                envio.Modalidad,
                envio.PesoKg,
                envio.DistanciaKm,
                envio.Destino,
                envio.Costo,
                envio.DiasEstimados,
                envio.Estado
            ));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
})
.WithName("CrearEnvio");

// ✅ GET /api/envios — Listar todos
app.MapGet("/api/envios", (EnvioService servicio) =>
{
    var envios = servicio.ListarTodos()
        .Select(e => new EnvioRespuestaDto(
            e.Id, e.Modalidad, e.PesoKg, e.DistanciaKm,
            e.Destino, e.Costo, e.DiasEstimados, e.Estado));
    return Results.Ok(envios);
})
.WithName("ListarEnvios");

// ✅ GET /api/envios/{id} — Obtener por ID
app.MapGet("/api/envios/{id}", (int id, EnvioService servicio) =>
{
    var envio = servicio.BuscarPorId(id);
    if (envio == null) return Results.NotFound(new { mensaje = "Envío no encontrado" });
    
    return Results.Ok(new EnvioRespuestaDto(
        envio.Id, envio.Modalidad, envio.PesoKg, envio.DistanciaKm,
        envio.Destino, envio.Costo, envio.DiasEstimados, envio.Estado));
})
.WithName("ObtenerEnvio");

// ✅ GET /api/envios/{id}/cotizacion — Solo cotizar
app.MapGet("/api/envios/{id}/cotizacion", (int id, EnvioService servicio) =>
{
    try
    {
        var (costo, dias) = servicio.ObtenerCotizacion(id);
        return Results.Ok(new CotizacionDto(costo, dias));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { mensaje = "Envío no encontrado" });
    }
})
.WithName("ObtenerCotizacion");

// ✅ PUT /api/envios/{id} — Actualizar completo
app.MapPut("/api/envios/{id}", (int id, ActualizarEnvioDto dto, EnvioService servicio) =>
{
    try
    {
        var envio = servicio.Actualizar(id, dto.Modalidad, dto.PesoKg, dto.DistanciaKm, dto.Destino);
        return Results.Ok(new EnvioRespuestaDto(
            envio.Id, envio.Modalidad, envio.PesoKg, envio.DistanciaKm,
            envio.Destino, envio.Costo, envio.DiasEstimados, envio.Estado));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { mensaje = "Envío no encontrado" });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
})
.WithName("ActualizarEnvio");

// ✅ PATCH /api/envios/{id}/estado — Avanzar estado automáticamente
app.MapPatch("/api/envios/{id}/estado", (int id, EnvioService servicio) =>
{
    try
    {
        // Primero buscamos el envío para saber su estado actual
        var envio = servicio.BuscarPorId(id);
        if (envio == null) return Results.NotFound(new { mensaje = "Envío no encontrado" });
        
        // Determinamos el próximo estado permitido
        string? proximoEstado = envio.Estado switch
        {
            "PENDIENTE" => "DESPACHADO",
            "DESPACHADO" => "ENTREGADO",
            _ => null
        };
        
        if (proximoEstado == null)
            return Results.BadRequest(new { mensaje = $"No se puede avanzar desde el estado {envio.Estado}" });
        
        var resultado = servicio.CambiarEstado(id, proximoEstado);
        return Results.Ok(new { mensaje = $"Estado cambiado a {resultado.Estado}" });
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { mensaje = "Envío no encontrado" });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
})
.WithName("AvanzarEstado");

// ✅ DELETE /api/envios/{id} — Eliminar
app.MapDelete("/api/envios/{id}", (int id, EnvioService servicio) =>
{
    if (servicio.Eliminar(id))
        return Results.NoContent();
    return Results.NotFound(new { mensaje = "Envío no encontrado" });
})
.WithName("EliminarEnvio");

app.Run();
