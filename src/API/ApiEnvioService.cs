using Aplicacion.Modalidades;
using Persistencia.Entidades;
using Persistencia.Repositorios;

namespace API;

public sealed class ApiEnvioService
{
    private readonly Dictionary<string, IEnvioModalidad> _modalidades;
    private readonly IEnvioApiRepository _repositorio;

    public ApiEnvioService(
        IEnumerable<IEnvioModalidad> modalidades,
        IEnvioApiRepository repositorio)
    {
        _modalidades = modalidades.ToDictionary(
            modalidad => modalidad.Nombre,
            StringComparer.OrdinalIgnoreCase);
        _repositorio = repositorio;
    }

    public Envio Crear(SolicitudEnvio solicitud)
    {
        var datos = ValidarYCalcular(solicitud);
        var registro = new EnvioApiRecord(
            0,
            datos.Modalidad.Nombre,
            solicitud.PesoKg,
            solicitud.DistanciaKm,
            solicitud.Destino.Trim(),
            "PENDIENTE");

        var id = _repositorio.Crear(registro);
        return Proyectar(registro with { Id = id });
    }

    public IReadOnlyCollection<Envio> Listar()
    {
        return _repositorio.Listar().Select(Proyectar).ToArray();
    }

    public Envio? Buscar(int id)
    {
        var registro = _repositorio.Buscar(id);
        return registro is null ? null : Proyectar(registro);
    }

    public Envio? Actualizar(int id, SolicitudEnvio solicitud)
    {
        var actual = _repositorio.Buscar(id);
        if (actual is null)
        {
            return null;
        }

        var datos = ValidarYCalcular(solicitud);
        var actualizado = new EnvioApiRecord(
            id,
            datos.Modalidad.Nombre,
            solicitud.PesoKg,
            solicitud.DistanciaKm,
            solicitud.Destino.Trim(),
            actual.Estado);

        return _repositorio.Actualizar(actualizado)
            ? Proyectar(actualizado)
            : null;
    }

    public bool Eliminar(int id)
    {
        return _repositorio.Eliminar(id);
    }

    public Envio? CambiarEstado(int id, string nuevoEstado)
    {
        var actual = _repositorio.Buscar(id);
        if (actual is null)
        {
            return null;
        }

        var transicionPermitida =
            (actual.Estado == "PENDIENTE" && nuevoEstado == "DESPACHADO") ||
            (actual.Estado == "DESPACHADO" && nuevoEstado == "ENTREGADO");

        if (!transicionPermitida)
        {
            throw new ArgumentException(
                "Solo se permite pasar de PENDIENTE a DESPACHADO y de DESPACHADO a ENTREGADO.");
        }

        if (!_repositorio.CambiarEstado(id, actual.Estado, nuevoEstado))
        {
            return null;
        }

        return Proyectar(actual with { Estado = nuevoEstado });
    }

    private (IEnvioModalidad Modalidad, decimal Costo) ValidarYCalcular(
        SolicitudEnvio solicitud)
    {
        if (solicitud is null)
        {
            throw new ArgumentException("Los datos del envío son obligatorios.");
        }

        if (solicitud.PesoKg <= 0)
        {
            throw new ArgumentException("El peso debe ser mayor a cero.");
        }

        if (solicitud.DistanciaKm <= 0)
        {
            throw new ArgumentException("La distancia debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(solicitud.Destino))
        {
            throw new ArgumentException("El destino es obligatorio.");
        }

        var nombreModalidad = solicitud.Modalidad?.Trim().ToLowerInvariant();
        if (nombreModalidad == "estándar")
        {
            nombreModalidad = "estandar";
        }

        if (nombreModalidad is null || !_modalidades.TryGetValue(nombreModalidad, out var modalidad))
        {
            throw new ArgumentException("La modalidad debe ser estandar o express.");
        }

        return (modalidad, modalidad.CalcularCosto(solicitud.PesoKg, solicitud.DistanciaKm));
    }

    private Envio Proyectar(EnvioApiRecord registro)
    {
        var modalidad = _modalidades[registro.Modalidad];
        return new Envio(
            registro.Id,
            registro.Modalidad,
            registro.PesoKg,
            registro.DistanciaKm,
            registro.Destino,
            registro.Estado,
            modalidad.CalcularCosto(registro.PesoKg, registro.DistanciaKm),
            modalidad.DiasEstimados);
    }
}