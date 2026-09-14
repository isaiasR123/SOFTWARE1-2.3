public class EnvioEstandar : Envio
{
    public EnvioEstandar(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino)
        : base(id, cliente, paquete, direccionOrigen, direccionDestino)
    {
    }
}