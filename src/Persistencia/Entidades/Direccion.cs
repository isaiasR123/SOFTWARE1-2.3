namespace Persistencia.Entidades;

public class Direccion
{
    public int Id { get; set; }
    public string Calle { get; set; }
    public int Numero { get; set; }
    public string Ciudad { get; set; }

    public Direccion(int id, string calle, int numero, string ciudad)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(calle))
            throw new ArgumentException("La calle es obligatoria.");

        if (numero <= 0)
            throw new ArgumentException("El número debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(ciudad))
            throw new ArgumentException("La ciudad es obligatoria.");

        Id = id;
        Calle = calle;
        Numero = numero;
        Ciudad = ciudad;
    }
}