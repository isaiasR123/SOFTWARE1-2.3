using Aplicacion.Interfaces;
using Persistencia.Entidades;
using Persistencia.Repositorios;

namespace Aplicacion.Servicios;

public class EnvioService
{
    private readonly EnvioRepository _repositorio;
    private readonly Dictionary<string, IEnvioModalidad> _modalidades;

    public EnvioService(EnvioRepository repositorio, IEnumerable<IEnvioModalidad> modalidades)
    {
        _repositorio = repositorio;
        _modalidades = modalidades.ToDictionary(m => m.Nombre);
    }

    public Envio CrearEnvio(string modalidadNombre, decimal pesoKg, decimal distanciaKm, string destino)
    {
        if (!_modalidades.TryGetValue(modalidadNombre.ToLower(), out var modalidad))
            throw new ArgumentException($"Modalidad inválida: {modalidadNombre}");
        if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor que cero.");
        if (distanciaKm <= 0) throw new ArgumentException("La distancia debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(destino)) throw new ArgumentException("El destino es obligatorio.");

        var costo = modalidad.CalcularCosto(pesoKg, distanciaKm);

        var envio = new Envio
        {
            Modalidad = modalidadNombre.ToLower(),
            PesoKg = pesoKg,
            DistanciaKm = distanciaKm,
            Destino = destino,
            Costo = costo,
            DiasEstimados = modalidad.DiasEstimados,
            Estado = "PENDIENTE"
        };

        envio.Id = _repositorio.Crear(envio);
        return envio;
    }

    public List<Envio> ListarTodos() => _repositorio.Listar();
    public Envio? BuscarPorId(int id) => _repositorio.Buscar(id);

    public (decimal costo, int diasEstimados) ObtenerCotizacion(int id)
    {
        var envio = _repositorio.Buscar(id) ?? throw new KeyNotFoundException($"No existe el envío {id}");
        if (!_modalidades.TryGetValue(envio.Modalidad, out var modalidad))
            throw new InvalidOperationException($"Modalidad desconocida: {envio.Modalidad}");
        return (modalidad.CalcularCosto(envio.PesoKg, envio.DistanciaKm), modalidad.DiasEstimados);
    }

    public Envio Actualizar(int id, string modalidadNombre, decimal pesoKg, decimal distanciaKm, string destino)
    {
        var envio = _repositorio.Buscar(id) ?? throw new KeyNotFoundException($"No existe el envío {id}");
        if (!_modalidades.TryGetValue(modalidadNombre.ToLower(), out var modalidad))
            throw new ArgumentException($"Modalidad inválida: {modalidadNombre}");
        if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor que cero.");
        if (distanciaKm <= 0) throw new ArgumentException("La distancia debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(destino)) throw new ArgumentException("El destino es obligatorio.");

        envio.Modalidad = modalidadNombre.ToLower();
        envio.PesoKg = pesoKg;
        envio.DistanciaKm = distanciaKm;
        envio.Destino = destino;
        envio.Costo = modalidad.CalcularCosto(pesoKg, distanciaKm);
        envio.DiasEstimados = modalidad.DiasEstimados;
        // Id y Estado se conservan

        _repositorio.Actualizar(envio);
        return envio;
    }

    public bool Eliminar(int id) => _repositorio.Eliminar(id);
}
