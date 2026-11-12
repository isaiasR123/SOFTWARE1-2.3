using Persistencia.Entidades;

namespace Persistencia.Repositorios;

public interface IEnvioRepository
{
    int Crear(Envio envio);
    List<Envio> Listar();
    Envio? Buscar(int id);
    bool Actualizar(Envio envio);
    bool Eliminar(int id);
}