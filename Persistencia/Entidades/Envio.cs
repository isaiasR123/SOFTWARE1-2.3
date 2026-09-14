using

    class pogram
    
public abstract class Envio
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public Paquete Paquete { get; set; }
    public Direccion DireccionOrigen { get; set; }
    public Direccion DireccionDestino { get; set; }

    public Envio(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion direccionOrigen,
        Direccion direccionDestino)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        if (cliente == null)
            throw new ArgumentException("El cliente es obligatorio.");

        if (paquete == null)
            throw new ArgumentException("El paquete es obligatorio.");

        if (direccionOrigen == null)
            throw new ArgumentException("La dirección de origen es obligatoria.");

        if (direccionDestino == null)
            throw new ArgumentException("La dirección de destino es obligatoria.");

        Id = id;
        Cliente = cliente;
        Paquete = paquete;
        DireccionOrigen = direccionOrigen;
        DireccionDestino = direccionDestino;
    }
}
