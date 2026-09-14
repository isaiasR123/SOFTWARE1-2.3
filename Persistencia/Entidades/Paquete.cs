public class Paquete
{
    public int Id { get; set; }
    public double Peso { get; set; }
    public string Descripcion { get; set; }

    public Paquete(int id, double peso, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        if (peso <= 0)
            throw new ArgumentException("El peso debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción no puede estar vacía.");

        Id = id;
        Peso = peso;
        Descripcion = descripcion;
    }
}