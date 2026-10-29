using Persistencia.Entidades;

namespace Persistencia.Repositorios;

public interface IEnvioApiRepository
{
    int Crear(EnvioApiRecord envio);
    IEnumerable<EnvioApiRecord> Listar();
    EnvioApiRecord? Buscar(int id);
    bool Actualizar(EnvioApiRecord envio);
    bool CambiarEstado(int id, string estadoActual, string estadoNuevo);
    bool Eliminar(int id);
}