using Aplicacion.Interfaces;
using Aplicacion.Modalidades;
using Aplicacion.Servicios;

namespace Tests;

public class ApiEnvioTests
{
    private static EnvioService CrearServicio()
    {
        return new EnvioService(
            new FakeEnvioRepository(),
            new IEnvioModalidad[]
            {
                new EnvioEstandarModalidad(),
                new EnvioExpressModalidad()
            });
    }

    [Fact]
    public void CrearEnvio_Estandar_CalculaCostoYPlazo()
    {
        var servicio = CrearServicio();

        var envio = servicio.CrearEnvio("estandar", 5, 10, "Córdoba");

        Assert.Equal(2100m, envio.Costo);
        Assert.Equal(5, envio.DiasEstimados);
    }

    [Fact]
    public void CrearEnvio_Express_CalculaCostoYPlazo()
    {
        var servicio = CrearServicio();

        var envio = servicio.CrearEnvio("express", 5, 10, "Córdoba");

        Assert.Equal(3950m, envio.Costo);
        Assert.Equal(2, envio.DiasEstimados);
    }

    [Theory]
    [InlineData("estandar", 0, 10, "Córdoba")]
    [InlineData("estandar", -5, 10, "Córdoba")]
    [InlineData("express", 5, 0, "Córdoba")]
    [InlineData("express", 5, -1, "Córdoba")]
    [InlineData("prioritario", 5, 10, "Córdoba")]
    [InlineData("estandar", 5, 10, " ")]
    public void CrearEnvio_RechazaDatosInvalidos(
        string modalidad, decimal pesoKg, decimal distanciaKm, string destino)
    {
        var servicio = CrearServicio();

        Assert.Throws<ArgumentException>(() =>
            servicio.CrearEnvio(modalidad, pesoKg, distanciaKm, destino));
    }

    [Fact]
    public void Actualizar_ConservaIdYEstado_RecalculaCotizacionYGuarda()
    {
        var servicio = CrearServicio();
        var creado = servicio.CrearEnvio("estandar", 5, 10, "Córdoba");

        var actualizado = servicio.Actualizar(creado.Id, "express", 2, 20, "Rosario");

        Assert.Equal(creado.Id, actualizado.Id);
        Assert.Equal("PENDIENTE", actualizado.Estado);
        Assert.Equal(3100m, actualizado.Costo);
        Assert.Equal(2, actualizado.DiasEstimados);

        var guardado = servicio.BuscarPorId(creado.Id);
        Assert.NotNull(guardado);
        Assert.Equal("express", guardado.Modalidad);
        Assert.Equal("Rosario", guardado.Destino);
        Assert.Equal(3100m, guardado.Costo);
    }

    [Fact]
    public void Actualizar_IdInexistente_LanzaKeyNotFoundException()
    {
        var servicio = CrearServicio();

        Assert.Throws<KeyNotFoundException>(() =>
            servicio.Actualizar(99, "estandar", 5, 10, "Córdoba"));
    }

    [Fact]
    public void ListarTodos_SinRegistros_DevuelveListaVacia()
    {
        var servicio = CrearServicio();

        Assert.Empty(servicio.ListarTodos());
    }

    [Fact]
    public void BuscarPorId_IdInexistente_DevuelveNull()
    {
        var servicio = CrearServicio();

        Assert.Null(servicio.BuscarPorId(99));
    }

    [Fact]
    public void ObtenerCotizacion_DevuelveCostoYDiasEstimados()
    {
        var servicio = CrearServicio();
        var creado = servicio.CrearEnvio("estandar", 5, 10, "Córdoba");

        var cotizacion = servicio.ObtenerCotizacion(creado.Id);

        Assert.Equal(2100m, cotizacion.costo);
        Assert.Equal(5, cotizacion.diasEstimados);
    }

    [Fact]
    public void ObtenerCotizacion_IdInexistente_LanzaKeyNotFoundException()
    {
        var servicio = CrearServicio();

        Assert.Throws<KeyNotFoundException>(() => servicio.ObtenerCotizacion(99));
    }

    [Fact]
    public void Eliminar_BorraElEnvio_YLaSegundaVezDevuelveFalse()
    {
        var servicio = CrearServicio();
        var creado = servicio.CrearEnvio("estandar", 5, 10, "Córdoba");

        Assert.True(servicio.Eliminar(creado.Id));
        Assert.Null(servicio.BuscarPorId(creado.Id));
        Assert.False(servicio.Eliminar(creado.Id));
    }
}