namespace Persistencia.Entidades;

public class EnvioPrioritario : Envio
{
    public EnvioPrioritario(
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
        return Paquete.Peso * 20 + Distancia * 4;
    }

    public override double CalcularTiempoEntrega()
    {
        return 1;
    }
}