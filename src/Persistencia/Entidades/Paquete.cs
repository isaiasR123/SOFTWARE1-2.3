namespace Persistencia.Entidades;

public class Paquete
{
    public int Id { get; set; }
    public double Peso { get; set; }
    public double Largo { get; set; }
    public double Ancho { get; set; }
    public double Alto { get; set; }
    public string Descripcion { get; set; }

    public Paquete(
        int id,
        double peso,
        double largo,
        double ancho,
        double alto,
        string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a cero.");

        if (peso <= 0)
            throw new ArgumentException("El peso debe ser mayor a cero.");

        if (largo <= 0 || ancho <= 0 || alto <= 0)
            throw new ArgumentException("Las dimensiones deben ser mayores a cero.");

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es obligatoria.");

        Id = id;
        Peso = peso;
        Largo = largo;
        Ancho = ancho;
        Alto = alto;
        Descripcion = descripcion;
    }
}