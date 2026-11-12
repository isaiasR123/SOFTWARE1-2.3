
using Aplicacion.Modalidades;
using Xunit;

namespace Tests;

public class EnvioExpressTests
{
    [Fact]
    public void EnvioExpress_DebeCalcularCostoCorrectamente()
    {
        // Arrange
        EnvioExpressModalidad modalidad = new EnvioExpressModalidad();

        // Act
        decimal resultado = modalidad.CalcularCosto(5, 10);

        // Assert
        Assert.Equal(3950, resultado);
    }

    [Fact]
    public void EnvioExpress_DebeCalcularTiempoCorrectamente()
    {
        // Arrange
        EnvioExpressModalidad modalidad = new EnvioExpressModalidad();

        // Act
        int resultado = modalidad.DiasEstimados;

        // Assert
        Assert.Equal(2, resultado);
    }
}
