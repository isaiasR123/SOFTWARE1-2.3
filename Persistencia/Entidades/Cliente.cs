public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public Direccion Direccion { get; set; }

    public Cliente(int id, string nombre, string email, Direccion direccion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío.");

        Id = id;
        Nombre = nombre;
        Email = email;
        Direccion = direccion;
    }
}