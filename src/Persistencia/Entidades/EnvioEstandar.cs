namespace Persistencia.Entidades;

public class EnvioEstandar : Envio
{
    public EnvioEstandar(
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
        return Paquete.Peso * 10 + Distancia * 2;
    }

    public override double CalcularTiempoEntrega()
    {
        return 5;
    }
}