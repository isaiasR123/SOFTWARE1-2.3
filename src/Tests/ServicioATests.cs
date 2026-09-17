using Persistencia.Entidades;
using Xunit;

namespace Tests;

public class ServicioATests
{
private Direccion CrearDireccion()
{
return new Direccion(
1,
"Av. Siempre Viva",
123,
"Buenos Aires");
}


private Cliente CrearCliente()
{
    return new Cliente(
        1,
        "Juan Perez",
        "juan@gmail.com",
        CrearDireccion());
}

private Paquete CrearPaquete()
{
    return new Paquete(
        1,
        5,
        20,
        10,
        10,
        "Paquete de prueba");
}

[Fact]
public void EnvioEstandar_DebeCalcularCostoCorrectamente()
{
    Envio envio = new EnvioEstandar(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularCosto();

    Assert.Equal(70, resultado);
}

[Fact]
public void EnvioEstandar_DebeCalcularTiempoCorrectamente()
{
    Envio envio = new EnvioEstandar(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularTiempoEntrega();

    Assert.Equal(5, resultado);
}


}


