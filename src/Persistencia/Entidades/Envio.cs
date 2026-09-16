namespace Persistencia.Entidades;

public abstract class Envio
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public Paquete Paquete { get; set; }
    public Direccion DireccionOrigen { get; set; }
    public Direccion DireccionDestino { get; set; }
    public double Distancia { get; set; }

    protected Envio(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino,
        double distancia)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a cero.");

        if (cliente == null)
            throw new ArgumentException("El cliente es obligatorio.");

        if (paquete == null)
            throw new ArgumentException("El paquete es obligatorio.");

        if (direccionOrigen == null)
            throw new ArgumentException("La dirección de origen es obligatoria.");

        if (direccionDestino == null)
            throw new ArgumentException("La dirección de destino es obligatoria.");

        if (distancia <= 0)
            throw new ArgumentException("La distancia debe ser mayor a cero.");

        Id = id;
        Cliente = cliente;
        Paquete = paquete;
        DireccionOrigen = direccionOrigen;
        DireccionDestino = direccionDestino;
        Distancia = distancia;
    }

    public abstract double CalcularCosto();

    public abstract double CalcularTiempoEntrega();
}