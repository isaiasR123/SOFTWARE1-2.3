public class EnvioPrioritario : Envio
{
    public EnvioPrioritario(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino)
        : base(id, cliente, paquete, direccionOrigen, direccionDestino)
    {
    }
}