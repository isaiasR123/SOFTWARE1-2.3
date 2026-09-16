namespace Persistencia.Entidades;

public class EnvioExpress : Envio
{
    public EnvioExpress(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino,
        double distancia)
        : base(
            id,
            cliente,
            paquete,
            direccionOrigen,
            direccionDestino,
            distancia)
    {
    }

    public override double CalcularCosto()
    {
        return Paquete.Peso * 15 + Distancia * 3;
    }

    public override double CalcularTiempoEntrega()
    {
        return 2;
    }
}