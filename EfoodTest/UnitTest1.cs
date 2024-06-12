using NUnit.Framework;
using EFood.Modelos;
using System.ComponentModel.DataAnnotations;

namespace EFood.Tests
{
    [TestFixture]
    public class LineaComidaTests
    {
        [Test]
        public void LineaComida_Debe_Validar_Nombre_Requerido()
        {
            // Arrange
            var lineaComida = new LineaComida();

            // Act
            bool esValido = Validator.TryValidateObject(lineaComida, new ValidationContext(lineaComida), null, true);

            // Assert
            Assert.IsFalse(esValido);
        }

        [Test]
        public void LineaComida_Debe_Validar_Longitud_Maxima_Nombre()
        {
            // Arrange
            var lineaComida = new LineaComida { Nombre = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam non turpis sed" };

            // Act
            bool esValido = Validator.TryValidateObject(lineaComida, new ValidationContext(lineaComida), null, true);

            // Assert
            Assert.IsFalse(esValido);
        }
    }
}
