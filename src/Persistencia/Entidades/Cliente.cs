namespace Persistencia.Entidades;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public Direccion Direccion { get; set; }

    public Cliente(int id, string nombre, string email, Direccion direccion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es obligatorio.");

        if (direccion == null)
            throw new ArgumentException("La dirección es obligatoria.");

        Id = id;
        Nombre = nombre;
        Email = email;
        Direccion = direccion;
    }
}