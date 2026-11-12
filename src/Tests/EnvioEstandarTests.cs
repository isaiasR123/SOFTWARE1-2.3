
using Aplicacion.Modalidades;
using Xunit;

namespace Tests;

public class EnvioEstandarTests
{
    [Fact]
    public void EnvioEstandar_DebeCalcularCostoCorrectamente()
    {
        // Arrange
        EnvioEstandarModalidad modalidad = new EnvioEstandarModalidad();

        // Act
        decimal resultado = modalidad.CalcularCosto(5, 10);

        // Assert
        Assert.Equal(2100, resultado);
    }

    [Fact]
    public void EnvioEstandar_DebeCalcularTiempoCorrectamente()
    {
        // Arrange
        EnvioEstandarModalidad modalidad = new EnvioEstandarModalidad();

        // Act
        int resultado = modalidad.DiasEstimados;

        // Assert
        Assert.Equal(5, resultado);
    }
}
