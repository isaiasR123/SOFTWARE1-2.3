public class Direccion
{
    public string Calle { get; set; }
    public int Numero { get; set; }
    public string Ciudad { get; set; }

    public Direccion(string calle, int numero, string ciudad)
    {
        if (string.IsNullOrWhiteSpace(calle))
            throw new ArgumentException("La calle no puede estar vacía.");

        if (numero <= 0)
            throw new ArgumentException("El número debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(ciudad))
            throw new ArgumentException("La ciudad no puede estar vacía.");

        Calle = calle;
        Numero = numero;
        Ciudad = ciudad;
    }
}