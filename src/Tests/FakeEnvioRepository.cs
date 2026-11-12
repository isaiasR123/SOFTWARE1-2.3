using Persistencia.Entidades;
using Persistencia.Repositorios;

namespace Tests;

public class FakeEnvioRepository : IEnvioRepository
{
    private readonly Dictionary<int, Envio> _envios = new();
    private int _siguienteId = 1;

    public int Crear(Envio envio)
    {
        var id = _siguienteId++;
        var copia = Copiar(envio);
        copia.Id = id;
        _envios[id] = copia;
        return id;
    }

    public List<Envio> Listar() =>
        _envios.Values.OrderByDescending(e => e.Id).Select(Copiar).ToList();

    public Envio? Buscar(int id) =>
        _envios.TryGetValue(id, out var envio) ? Copiar(envio) : null;

    public bool Actualizar(Envio envio)
    {
        if (!_envios.ContainsKey(envio.Id)) return false;
        _envios[envio.Id] = Copiar(envio);
        return true;
    }

    public bool Eliminar(int id) => _envios.Remove(id);

    private static Envio Copiar(Envio e) => new()
    {
        Id = e.Id,
        Modalidad = e.Modalidad,
        PesoKg = e.PesoKg,
        DistanciaKm = e.DistanciaKm,
        Destino = e.Destino,
        Costo = e.Costo,
        DiasEstimados = e.DiasEstimados,
        Estado = e.Estado
    };
}