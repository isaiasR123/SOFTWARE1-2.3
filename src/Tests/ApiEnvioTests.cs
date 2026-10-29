using API;
using Aplicacion.Modalidades;
using Persistencia.Entidades;
using Persistencia.Repositorios;
using Xunit;

namespace Tests;

public class ApiEnvioTests
{
    private static ApiEnvioService CrearServicio()
    {
        return new ApiEnvioService(
        [
            new EnvioEstandarModalidad(),
            new EnvioExpressModalidad()
        ], new FakeEnvioApiRepository());
    }

    [Theory]
    [InlineData("estandar", 5, 10, 2100, 5)]
    [InlineData("express", 5, 10, 3950, 2)]
    public void Crear_CalculaCostoYPlazoSegunModalidad(
        string modalidad,
        decimal pesoKg,
        decimal distanciaKm,
        decimal costoEsperado,
        int diasEsperados)
    {
        var servicio = CrearServicio();

        var envio = servicio.Crear(new SolicitudEnvio(
            modalidad, pesoKg, distanciaKm, "Córdoba"));

        Assert.Equal(costoEsperado, envio.Costo);
        Assert.Equal(diasEsperados, envio.DiasEstimados);
    }

    [Fact]
    public void Actualizar_ConservaIdentificadorYEstado_YRecalculaCotizacion()
    {
        var servicio = CrearServicio();
        var creado = servicio.Crear(new SolicitudEnvio(
            "estandar", 5, 10, "Córdoba"));

        var actualizado = servicio.Actualizar(creado.Id, new SolicitudEnvio(
            "express", 2, 20, "Rosario"));

        Assert.NotNull(actualizado);
        Assert.Equal(creado.Id, actualizado.Id);
        Assert.Equal(creado.Estado, actualizado.Estado);
        Assert.Equal(3100m, actualizado.Costo);
        Assert.Equal(2, actualizado.DiasEstimados);
    }

    [Theory]
    [InlineData("estandar", 0, 10, "Córdoba")]
    [InlineData("express", 5, 0, "Córdoba")]
    [InlineData("prioritario", 5, 10, "Córdoba")]
    [InlineData("estandar", 5, 10, " ")]
    public void Crear_RechazaDatosInvalidos(
        string modalidad,
        decimal pesoKg,
        decimal distanciaKm,
        string destino)
    {
        var servicio = CrearServicio();

        Assert.Throws<ArgumentException>(() => servicio.Crear(new SolicitudEnvio(
            modalidad, pesoKg, distanciaKm, destino)));
    }

    [Fact]
    public void Eliminar_BorraElEnvioYNoEncuentraElIdLuego()
    {
        var servicio = CrearServicio();
        var envio = servicio.Crear(new SolicitudEnvio(
            "estandar", 5, 10, "Córdoba"));

        Assert.True(servicio.Eliminar(envio.Id));
        Assert.Null(servicio.Buscar(envio.Id));
        Assert.False(servicio.Eliminar(envio.Id));
    }

    [Fact]
    public void Actualizar_IdInexistenteDevuelveNullAunqueLosDatosSeanInvalidos()
    {
        var servicio = CrearServicio();

        var actualizado = servicio.Actualizar(99, new SolicitudEnvio(
            "invalida", 0, 0, ""));

        Assert.Null(actualizado);
    }

    [Fact]
    public void Crear_IniciaEnvioEnEstadoPendiente()
    {
        var servicio = CrearServicio();

        var envio = servicio.Crear(new SolicitudEnvio(
            "estandar", 1, 1, "Córdoba"));

        Assert.Equal("PENDIENTE", envio.Estado);
    }

    [Fact]
    public void CambiarEstado_PermiteTransicionesEnOrden()
    {
        var servicio = CrearServicio();
        var creado = servicio.Crear(new SolicitudEnvio(
            "estandar", 1, 1, "Córdoba"));

        var despachado = servicio.CambiarEstado(creado.Id, "DESPACHADO");
        var entregado = servicio.CambiarEstado(creado.Id, "ENTREGADO");

        Assert.Equal("DESPACHADO", despachado?.Estado);
        Assert.Equal("ENTREGADO", entregado?.Estado);
    }

    [Theory]
    [InlineData("ENTREGADO")]
    [InlineData("PENDIENTE")]
    [InlineData("CANCELADO")]
    public void CambiarEstado_RechazaTransicionesNoPermitidas(string nuevoEstado)
    {
        var servicio = CrearServicio();
        var envio = servicio.Crear(new SolicitudEnvio(
            "estandar", 1, 1, "Córdoba"));

        Assert.Throws<ArgumentException>(() =>
            servicio.CambiarEstado(envio.Id, nuevoEstado));
    }

    [Fact]
    public void CambiarEstado_NoPermiteRetrocederDesdeDespachado()
    {
        var servicio = CrearServicio();
        var envio = servicio.Crear(new SolicitudEnvio(
            "estandar", 1, 1, "Córdoba"));
        servicio.CambiarEstado(envio.Id, "DESPACHADO");

        Assert.Throws<ArgumentException>(() =>
            servicio.CambiarEstado(envio.Id, "PENDIENTE"));
    }

    [Fact]
    public void CambiarEstado_IdInexistenteDevuelveNull()
    {
        var servicio = CrearServicio();

        Assert.Null(servicio.CambiarEstado(99, "DESPACHADO"));
    }

    private sealed class FakeEnvioApiRepository : IEnvioApiRepository
    {
        private readonly Dictionary<int, EnvioApiRecord> _envios = [];
        private int _siguienteId;

        public int Crear(EnvioApiRecord envio)
        {
            var id = ++_siguienteId;
            _envios.Add(id, envio with { Id = id });
            return id;
        }

        public IEnumerable<EnvioApiRecord> Listar() => _envios.Values.ToArray();

        public EnvioApiRecord? Buscar(int id) => _envios.GetValueOrDefault(id);

        public bool Actualizar(EnvioApiRecord envio)
        {
            if (!_envios.ContainsKey(envio.Id))
            {
                return false;
            }

            _envios[envio.Id] = envio;
            return true;
        }

        public bool CambiarEstado(int id, string estadoActual, string estadoNuevo)
        {
            if (!_envios.TryGetValue(id, out var envio) || envio.Estado != estadoActual)
            {
                return false;
            }

            _envios[id] = envio with { Estado = estadoNuevo };
            return true;
        }

        public bool Eliminar(int id) => _envios.Remove(id);
    }
}