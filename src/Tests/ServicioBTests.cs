using Persistencia.Entidades;
using Xunit;

namespace Tests;

public class ServicioBTests
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
public void EnvioExpress_DebeCalcularCostoCorrectamente()
{
    Envio envio = new EnvioExpress(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularCosto();

    Assert.Equal(105, resultado);
}

[Fact]
public void EnvioExpress_DebeCalcularTiempoCorrectamente()
{
    Envio envio = new EnvioExpress(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularTiempoEntrega();

    Assert.Equal(2, resultado);
}

[Fact]
public void EnvioPrioritario_DebeCalcularCostoCorrectamente()
{
    Envio envio = new EnvioPrioritario(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularCosto();

    Assert.Equal(140, resultado);
}

[Fact]
public void EnvioPrioritario_DebeCalcularTiempoCorrectamente()
{
    Envio envio = new EnvioPrioritario(
        1,
        CrearCliente(),
        CrearPaquete(),
        CrearDireccion(),
        CrearDireccion(),
        10);

    double resultado = envio.CalcularTiempoEntrega();

    Assert.Equal(1, resultado);
}


}
