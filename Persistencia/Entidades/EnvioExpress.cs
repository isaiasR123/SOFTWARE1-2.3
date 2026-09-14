public class EnvioExpress : Envio
{
    public EnvioExpress(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino)
        : base(id, cliente, paquete, direccionOrigen, direccionDestino)
    {
    }
}